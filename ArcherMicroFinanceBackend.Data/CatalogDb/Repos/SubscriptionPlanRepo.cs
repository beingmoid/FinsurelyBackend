


using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramBackend.Data.CatalogDb;
using AutoMapper;
using System.Linq;
using NukesLab.Core.Common;
using static NukesLab.Core.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace PanoramBackend.Data.CatalogDb.Repos
{
    public class StripePlanRequestDTO
    {
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Interval { get; set; }
        public string StripeSecretKey { get; set; }
        public bool IsRecurring { get; set; }

        public string StripeProductId { get; set; }
        public string StripePlanId { get; set; }
    }
    public class SubscriptionPlanRepo : EFRepository<SubscriptionPlans, int>, ISubscriptionPlanRepo
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        public SubscriptionPlanRepo(IMapper mapper ,IServiceProvider serviceProvider, CatalogDbContext requestScope) : base(requestScope)
        {
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }





        //Get All Subscription Plans
        public async Task<IEnumerable<SubscriptionPlans>> GetSubscriptionPlans()
        {
            IEnumerable<SubscriptionPlans> model;
            if (Utils.GetRole(_serviceProvider) != Roles.Admin)
            {
                var orderRepo = _serviceProvider.GetRequiredService<OrderRepo>();
                var lastOrder = (await orderRepo.Get(x=>x.Include(x=>x.SubscriptionPlan), p => !p.IsFree && p.CompanyGUID == Utils.GetTenantId(_serviceProvider))).FirstOrDefault();                if (lastOrder != null)
                    model = await this.Get(x => x.Include(p => p.BillingPlan), p => p.Currency == lastOrder.SubscriptionPlan.Currency);
                else
                    model = await this.Get(x => x.Include(p => p.BillingPlan));
            }
            else
            {
                model = await this.Get(x=>x.Include(p => p.BillingPlan));
            }
            return _mapper.Map<IEnumerable<SubscriptionPlans>>(model);
        }

        //Get Subscription Plan by Plan GUID
        public async Task<SubscriptionPlans> GetSubscriptionPlanByKey(string key)
        {
            return _mapper.Map<SubscriptionPlans>(await this.Get(x => x.Include(p => p.BillingPlan), p => p.SubscriptionPlanKey == key));
        }

        //Post Subscription Plan based on condition i.e. if plan name already exists under same billing type then plan will not be saved on db.
        public async Task Post(SubscriptionPlans model)
        {
            //condition to check is name already exists under same billing type.
            var isNameExist = (await this. Get()).Count(p => p.BillingPlanId == model.BillingPlanId && p.Name == model.Name.Trim()) > 0;
            if (isNameExist)
            {
                OtherConstants.isSuccessful = false;
                OtherConstants.responseMsg = "Plan Name already exist in selected billing plan type";
            }
            else
            {
                var response = _serviceProvider.GetRequiredService<IStripeService>().CreatePlan(new StripePlanRequestDTO()
                {
                    Amount = model.PricePerUser,
                    Currency = model.Currency.ToLower(),
                    PlanDescription = model.Description,
                    PlanName = model.Name,
                    StripeSecretKey = await _serviceProvider.GetRequiredService<IStripeConfigurationRepo>().GetStripeSecretKey(),
                    IsRecurring = model.IsRecurring,
                    Interval = model.IsRecurring ? GetInterval((await _serviceProvider.GetRequiredService<IBillingPlanRepo>().Get(x=>x.Id== model.BillingPlanId) ).SingleOrDefault().Name) : null
                });

                if (response.IsSuccessful)
                {
                    if (response.Data != null)
                    {
                        model.SubscriptionPlanKey = Guid.NewGuid().ToString();
                        model.SignUpURL = $"{DomainConfiguration.PortalAppDomain}#/auth/register?planid={model.SubscriptionPlanKey}&issignup=true";
                        model.CreateUserId = Utils.GetUserId(_serviceProvider);
                        model.StripeProductId = response.Data.StripeProductId;
                        model.StripePlanId = response.Data.StripePlanId;
                        await Post(_mapper.Map<SubscriptionPlans>(model));
                        OtherConstants.isSuccessful = true;
                        OtherConstants.responseMsg = "Subscription Saved Successfully!";
                    }
                    else
                    {
                        OtherConstants.isSuccessful = false;
                        OtherConstants.responseMsg = "Error Saving Subscription!";
                    }
                }
                else
                {
                    OtherConstants.isSuccessful = response.IsSuccessful;
                    OtherConstants.responseMsg = response.ErrorMessage;
                }
            }
        }

        //Update Subscription Plan based on condition i.e. if plan name already exists under same billing type then plan will not be saved on db.
        public async Task Put(int id, SubscriptionPlans model)
        {
            //condition to check is name already exists under same billing type.
            var isNameExist = (await this. Get()).Count(p => p.BillingPlanId == model.BillingPlanId && p.Name == model.Name.Trim() && p.Id != id) > 0;
            if (isNameExist)
            {
                OtherConstants.isSuccessful = false;
                OtherConstants.responseMsg = "Plan Name already exist in selected billing plan type";
            }
            else
            {
                var entity = await this.GetOne(id);
                if (entity != null)
                {
                    var response = _serviceProvider.GetRequiredService<IStripeService>().UpdatePlan(new StripePlanRequestDTO()
                    {
                        StripeProductId = entity.StripeProductId,
                        StripePlanId = entity.StripePlanId,
                        Amount = model.PricePerUser,
                        Currency = model.Currency.ToLower(),
                        PlanDescription = model.Description,
                        PlanName = model.Name,
                        StripeSecretKey = await  _serviceProvider.GetRequiredService<IStripeConfigurationRepo>().GetStripeSecretKey(),
                        IsRecurring = model.IsRecurring,
                        Interval = model.IsRecurring ? GetInterval((await _serviceProvider.GetRequiredService<IBillingPlanRepo>().Get(x=> x.Id==model.BillingPlanId)).SingleOrDefault().Name) : null
                    });

                    if (response.IsSuccessful)
                    {
                        entity.FreeUsersAllowed = model.FreeUsersAllowed;
                        entity.BillingPlanId = model.BillingPlanId;
                        entity.EditUserId = Utils.GetUserId(_serviceProvider);
                        entity.EditTime = DateTime.UtcNow;
                        entity.Description = model.Description;
                        entity.IsDeleted = false;
                        entity.Name = model.Name;
                        entity.IsEnabled = model.IsEnabled;
                        entity.IsRecurring = model.IsRecurring;
                        entity.PricePerUser = model.PricePerUser;
                        entity.Currency = model.Currency;
                        entity.Storage = model.Storage;
                        entity.FreeDays = model.FreeDays;
                        entity.StripePlanId = response.Data.StripePlanId;
                        entity.StripeProductId = response.Data.StripeProductId;
                       await this. Put(entity.Id,entity);
                    }
                    else
                    {
                        OtherConstants.isSuccessful = response.IsSuccessful;
                        OtherConstants.responseMsg = response.ErrorMessage;
                    }
                }
            }
        }

        //Check plan under condition i.e. plan is only editable if there are no paid users under specified plan. 
        public async Task<bool> CheckIsPlanEditable(int id)
        {
            var entity = await this.GetOne(id);
            if (entity != null)
            {
                var orderRepo = _serviceProvider.GetRequiredService<IOrderRepo>();

                //condition to check paid users.
                var areOrdersExist = (await orderRepo.Get(p => p.SubscriptionPlanId == id && !p.IsFree && p.EndDate > DateTime.UtcNow)).Any();
                if (areOrdersExist)
                {
                    OtherConstants.isSuccessful = false;
                    OtherConstants.responseMsg = "Users already subscribed to current plan";
                }
                else
                {
                    OtherConstants.isSuccessful = true;
                }
            }

            return OtherConstants.isSuccessful;
        }

        //Delete plan under condition i.e. plan is only removable if there are no paid users under specified plan. 
        public async Task<bool> Delete(int id)
        {
            var entity = await this.GetOne(id);
            if (entity != null)
            {
                var response = _serviceProvider.GetRequiredService<IStripeService>().DeletePlan(new StripePlanRequestDTO()
                {
                    StripeProductId = entity.StripeProductId,
                    StripePlanId = entity.StripePlanId,
                    StripeSecretKey =await _serviceProvider.GetRequiredService<IStripeConfigurationRepo>().GetStripeSecretKey(),
                });

                if (response.IsSuccessful)
                {
                    var orderRepo = _serviceProvider.GetRequiredService<IOrderRepo>();
                   
                    var areOrdersExist = (await orderRepo.Get(p => p.SubscriptionPlanId == id && p.EndDate > DateTime.UtcNow)).Any();

                    if (areOrdersExist)
                    {
                        OtherConstants.isSuccessful = false;
                        OtherConstants.responseMsg = "Users already subscribed to current plan";
                    }
                    else
                    {
                       
                       await this.Delete(entity.Id, entity);
                   
                       
                        OtherConstants.isSuccessful = true;
                    }
                }
                else
                {
                    OtherConstants.isSuccessful = response.IsSuccessful;
                    OtherConstants.responseMsg = response.ErrorMessage;
                }
            }
            return OtherConstants.isSuccessful;
        }

        private string GetInterval(string name)
        {
            return BillingPlans.Monthly == name ? "month" : "year";
        }

}
    public interface ISubscriptionPlanRepo : IEFRepository<SubscriptionPlans, int>
    {

    }
}
