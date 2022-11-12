using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
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
