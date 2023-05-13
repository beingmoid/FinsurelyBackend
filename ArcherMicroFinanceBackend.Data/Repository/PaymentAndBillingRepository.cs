using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class PaymentAndBillingRepository : EFRepository<PaymentAndBilling, int>, IPaymentAndBillingRepository
    {
        public PaymentAndBillingRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IPaymentAndBillingRepository : IEFRepository<PaymentAndBilling, int>
    {

    }
}
