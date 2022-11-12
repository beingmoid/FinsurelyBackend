


using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramBackend.Data.CatalogDb;
using System.Threading.Tasks;
using System.Linq;
using Stripe;
using NukesLab.Core.Common;
using static NukesLab.Core.Common.Constants;

namespace PanoramBackend.Data.CatalogDb.Repos
{
    public class StripeConfigurationRepo : EFRepository<StripeConfigurations, int>, IStripeConfigurationRepo
    {
        private IServiceProvider _serviceProvider;

        public StripeConfigurationRepo(CatalogDbContext requestScope, IServiceProvider serviceProvider) : base(requestScope)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<string> GetStripePublishKey()
        {
            return (await Get()).FirstOrDefault().StripePublishableKey;
        }

        public async Task<string> GetStripeSecretKey()
        {
            return (await Get()).FirstOrDefault().StripeSecretKey;
        }
        public async Task<StripeConfigurations> GetStripeConfiguration()
        {
            return ((await this.Get()).FirstOrDefault());
        }
        public async Task Put(int id, StripeConfigurations model)
        {
            var entity = await this.GetOne(id);
            if (entity != null)
            {
                var options = new RequestOptions
                {
                    ApiKey = model.StripeSecretKey
                };
                var service = new BalanceService();
                bool isValid = false;
                try
                {
                    Balance balance = service.Get(options);
                    var code = balance.StripeResponse.StatusCode;
                    if (code == System.Net.HttpStatusCode.OK)
                    {
                        isValid = true;
                    }
                }
                catch (Exception)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    entity.EditTime = DateTime.UtcNow;
                    entity.EditUserId = Utils.GetUserId(_serviceProvider);
                    entity.StripePublishableKey = model.StripePublishableKey;
                    entity.StripeSecretKey = model.StripeSecretKey;
                    //entity.StripeWebhookSecretKey = model.StripeWebhookSecretKey;
                    await this.Put(entity.Id,entity);
                    OtherConstants.isSuccessful = true;
                    if (entity.StripePublishableKey.ToLower().Contains("test"))
                    {
                        OtherConstants.responseMsg = "You have successfully updated Stripe Test Configurations!";
                    }
                    else
                    {
                        OtherConstants.responseMsg = "You have successfully updated Stripe Live Configurations!";
                    }

                }
                else
                {
                    OtherConstants.isSuccessful = false;
                    OtherConstants.responseMsg = "Secret Key is invalid";
                }
            }
        }

        public Charge ChargeAmount(long amount, string currency, string stripeToken)
        {
            var options = new ChargeCreateOptions
            {
                Amount = amount,
                Currency = currency,
                Source = stripeToken,
            };

            var charge = new ChargeService().Create(options);

            if (charge.Status == "succeeded")
                return charge;

            return null;
        }
    }
    public interface IStripeConfigurationRepo : IEFRepository<StripeConfigurations, int>
    {
        Task<string> GetStripePublishKey();
        Task<string> GetStripeSecretKey();
        Charge ChargeAmount(long amount, string currency, string stripeToken);
        Task<StripeConfigurations> GetStripeConfiguration();
        Task Put(int id, StripeConfigurations model);


    }
}
