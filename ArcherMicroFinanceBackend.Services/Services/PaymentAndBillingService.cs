using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class PaymentAndBillingService : BaseService<PaymentAndBilling, int>, IPaymentAndBillingService
    {
        public PaymentAndBillingService(RequestScope scopeContext, IPaymentAndBillingRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IPaymentAndBillingService : IBaseService<PaymentAndBilling, int>
    {

    }
}
