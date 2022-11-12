using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
