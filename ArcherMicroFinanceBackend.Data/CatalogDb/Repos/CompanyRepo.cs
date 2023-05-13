


//using PanoramaBackend.Data;
//using PanoramaBackend.Data.Entities;
//using NukesLab.Core.Repository;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using PanoramaBackend.Data.CatalogDb;
//using static NukesLab.Core.Common.Constants;
//using Microsoft.AspNetCore.Hosting;
//using System.IO;
//using Microsoft.Extensions.DependencyInjection;
//using NukesLab.Core.Common.EmailService;
//using System.Threading.Tasks;
//using NukesLab.Core.Common;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using PanoramaBackend.Data.CatalogDb.Repos;

//namespace PanoramaBackend.Data.CatalogDb.Repos
//{
//    public class OnboardingTenant
//    {

//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string Contact { get; set; }

//        public string Password { get; set; }
//        public string CompanyName { get; set; }

//        public string CompanyURL { get; set; }


//        public string BlobImageURI { get; set; }


//        public string ImageName { get; set; }
//        public byte[] CompanyGUID { get; set; }
//        public string PlanId { get; set; }
//        public int SubscriptionPlanId { get; set; }
//        public string SubscriptionPlanName { get; set; }
//        public string CardName { get; set; }
//        public string CardNumber { get; set; }
//        public int ExpiryMonth { get; set; }
//        public int ExpiryYear { get; set; }
//        public int PostalCode { get; set; }
//        public string StripeToken { get; set; }
//        public bool IsEmailVerified { get; set; }
//        public bool IsSignupCompleted { get; set; }


//    }
//    public class CompanyRepo : EFRepository<Company, int>, ICompanyRepo
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public ITokenService _tokenService { get; }

//        public CompanyRepo(CatalogDbContext requestScope, IServiceProvider serviceProvider,ITokenService tokenService) : base(requestScope)
//        {
//            _serviceProvider = serviceProvider;
//            _tokenService = tokenService;
//        }

//        public async Task RegisterUsingEmail(string email,int planId)
//        {
//            var companyRepo = _serviceProvider.GetRequiredService<ICompanyRepo>();
//            var company = new Company()
//            {
//                Email = email
//              , CompanyGUID = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString())
//            };

//            List < Company >li = new List<Company>();
//            li.Add(company);
//            await companyRepo.Insert(li);
//            if (await companyRepo.SaveChanges())
//            {
        
//                var token =  _tokenService.GenerateAccessToken(email);
//                var url = $"{DomainConfiguration.PortalAppDomain}#/signup/create-account?userid={company.CompanyGUID}&token={token}&planid={planId}";
//                var body = CreateEmailTemplate(url, EmailTemplateConfiguration.VerifyEmailDescription, EmailTemplateConfiguration.VerifyEmailButtonTitle, EmailTemplateConfiguration.VerifyEmailMessage, EmailTemplateConfiguration.VerifyEmailAddress);
//                if (!string.IsNullOrWhiteSpace(body))
//                {
//                    var isEmailSent =  _serviceProvider.GetRequiredService<IEmailService>().SendEmailWithoutTemplate(email, "Verify Email", body, true);
//                    if ( isEmailSent)
//                        OtherConstants.isSuccessful = true;
//                    else
//                        OtherConstants.isSuccessful = false;
//                }
//                else
//                    OtherConstants.isSuccessful = false;
//            }
//            else
//                OtherConstants.isSuccessful = false;

        
//        }


//        public async Task TokenVerfication(string email, string token)
//        {
            
//                var verified = _tokenService.ValidateToken(token);
//            if (verified)
//            {
//                var company = (await this.Get(x => x.Email == email)).SingleOrDefault();
//                company.EmailVerified = true;
//                await this.Update(company.Id, company);

//                if (await this.SaveChanges())
//                {
//                    OtherConstants.isSuccessful = true;
//                }
//                else
//                {
//                    OtherConstants.isSuccessful = false;
//                }

//            }
//            else
//            {
//                OtherConstants.isSuccessful = false;
//                OtherConstants.responseMsg = "Token expired , please try again";
//            }
               

//        }

//        public async Task OnboardingClient(OnboardingTenant tenant)
//        {
//            var subPlan =( await _serviceProvider.GetRequiredService<ISubscriptionPlanRepo>().Get(x=>x.Include(x => x.BillingPlan), x => x.BillingPlanId == tenant.SubscriptionPlanId));
//            //var _userManager = _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>();
//            var companyRepo = _serviceProvider.GetRequiredService<ICompanyRepo>();
//            var company =await companyRepo.GetOne(x => x.Email == tenant.Email && x.EmailVerified == true);
//         //   var usr = await _userManager.FindByEmailAsync(model.Email);
//            if (company != null)
//            {
//                var newTenant = new Tenants();
//                newTenant.Id = company.CompanyGUID;
//               // newTenant.Company = tenant.CompanyName;
//                newTenant.TenantName = tenant.Name;
//                newTenant.ServicePlan = tenant.SubscriptionPlanName;
//                newTenant.companyId = company.Id;
//                newTenant.LastUpdated = DateTime.UtcNow;
//                await _serviceProvider.GetRequiredService<ITenantsRepo>().Insert(new[] { newTenant });
              
                
//                var database = new Databases();
//                database.tenantId = newTenant.Id;
//                database.ServerName = ElasticPool.Server;
//                database.DatabaseName = newTenant.TenantName + "_"+newTenant.Id.ToString();
//                database.ConnectionString = DbConfig.DummyConnectionString.Replace("{dbname}", database.ServerName);
//                database.ElasticPoolName = ElasticPool.ElasticPoolName;
//                await _serviceProvider.GetRequiredService<IDatabasesRepo>().Insert(new[] { database });

//                if(await _serviceProvider.GetRequiredService<AMFContext>().Database.EnsureCreatedAsync())
//                {

//                }
//                var result = await _userManager.AddPasswordAsync(usr, model.Password);
//                if (result.Succeeded)
//                {
//                    var guid = Guid.NewGuid().ToString();
//                    await companyRepo.Post(new Company
//                    {
//                        CompanyName = "",
//                        CompanyURL = "",
//                        Contact = model.Contact,
//                        Email = model.Email,
//                        Name = model.Name,
//                        SubscriptionPlanId = subPlan.Id,
//                        CompanyGUID = guid,
//                        IsEnabled = true
//                    }, true);

//                    var company = companyRepo.Get().Include(p => p.SubscriptionPlan).Where(x => x.Email == model.Email).FirstOrDefault();
//                    await _userManager.AddToRoleAsync(usr, Roles.CompanyAdmin);
//                    company.CreatedBy = usr.Id;
//                    companyRepo.Put(company, true);

//                    await _serviceProvider.GetRequiredService<IUserCompanyRepo>().Post(new UserCompany
//                    {
//                        PhoneNo = model.Contact,
//                        LegalBusinessName = model.Name,
//                        CompanyEmail = model.Email,
//                        CompanyGUID = guid,
//                        IsEnabled = true
//                    }, true);

//                    usr.FirstName = model.FirstName;
//                    usr.LastName = model.LastName;
//                    usr.TenantId = company.CompanyGUID;
//                    usr.IsSignUpCompleted = true;
//                    usr.MemberStatus = true;
//                    usr.ProfileCompletion = 20;
//                    //usr.PhoneNumber = model.PlanId
//                    var res = await _userManager.UpdateAsync(usr);
//                    if (res.Succeeded)
//                    {
//                        var orderRepo = _serviceProvider.GetRequiredService<IOrdersRepo>();
//                        await orderRepo.Post(new Order()
//                        {
//                            CompanyId = company.Id,
//                            CompanyGUID = guid,
//                            CreatedDate = DateTime.UtcNow,
//                            IsFree = true,
//                            IsDeleted = false,
//                            Payment = 0,
//                            SubscriptionPlanId = company.SubscriptionPlanId,
//                            EndDate = DateTime.UtcNow.AddDays(Convert.ToDouble(company.SubscriptionPlan.FreeDays)),
//                            RecurringDate = null,
//                            SubscriptionStatus = EntityLayer.Helpers.Enums.SubscriptionStatus.Active,
//                            SubscriptionMessage = "Activated Subscription",
//                            NumberOfUsers = company.SubscriptionPlan.FreeUsersAllowed
//                        }, true);
//                        OtherConstants.isSuccessful = true;
//                    }
//                    else
//                        OtherConstants.isSuccessful = false;
//                }
//                else
//                    OtherConstants.isSuccessful = false;
//            }
//            return OtherConstants.isSuccessful;
//        }

//        //var cardInfo = new CardInfo()
//        //{
//        //    CardName = tenant.CardName,
//        //    CardNumber = tenant.CardNumber,
//        //    ExpiryMonth = tenant.ExpiryMonth,
//        //    ExpiryYear = tenant.ExpiryYear,
//        //    PostalCode = tenant.PostalCode,
//        //    SubscriptionPlanId = tenant.SubscriptionPlanId

//        //};
//        //await _serviceProvider.GetRequiredService<CardInfoRepo>().Insert(new[] { cardInfo });
//        //var stripekey = await _serviceProvider.GetRequiredService<IStripeConfigurationRepo>().GetStripeSecretKey();
//        //var subscription = (await _serviceProvider.GetRequiredService<SubscriptionPlanRepo>().Get(x => x.Id == tenant.SubscriptionPlanId)).SingleOrDefault();
//        //var stripe = _serviceProvider.GetRequiredService<IStripeService>().CreateCharge(
//        //    new Stripe.CreateChargeDTO() { 
//        //    StripeSecretKey=stripekey,
//        //    Amount=subscription.

//        //    })

//        //var company = new Company()
//        //{
//        //    Name=tenant.Name,
//        //    Contact=tenant.Contact,
//        //    CompanyName=tenant.CompanyName,
//        //    CompanyURL=tenant.CompanyURL,
//        //    CompanyGUID=tenant.CompanyGUID,
//        //    BlobImageURI=tenant.BlobImageURI,
//        //    ImageName=tenant.ImageName,
//        //    IsEnabled
//        //}
//    }
         
//        private string CreateEmailTemplate(string link, string description, string buttonTitle, string message, string address)
//        {
//        object _serviceProvider = null;
//        var _env = _serviceProvider.GetRequiredService<IHostingEnvironment>();
//            string path = Path.Combine(_env.ContentRootPath, "EmailTemplates\\EmailTemplateForVerifyAndReset.html");
//            if (System.IO.File.Exists(path))
//            {
//                string html = System.IO.File.ReadAllText(path);
//                html = html.Replace("{logoLink}", LogoConfigurations.PortalFrontEndLogo);
//                html = html.Replace("{description}", description);
//                html = html.Replace("{link}", link);
//                html = html.Replace("{buttonTitle}", buttonTitle);
//                html = html.Replace("{message}", message);
//                html = html.Replace("{address}", address);
//                return html;
//            }
//            return null;
//        }
//    }

//    internal class RegisterDTO
//    {
//        public RegisterDTO()
//        {
//        }

//        public int PlanId { get; set; }
//        public byte[] UserId { get; set; }
//    }

//    public interface ICompanyRepo : IEFRepository<Company, int>
//    {
//        Task TokenVerfication(string email, string token);
//        Task RegisterUsingEmail(string email, int planId);
//        Task OnboardingClient(OnboardingTenant tenant);
//    }
//}
