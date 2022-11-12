using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class PaymentCreditRepository : EFRepository<Payment, int>, IPaymentCreditRepository
    {
        public PaymentCreditRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IPaymentCreditRepository : IEFRepository<Payment, int>
    {

    }
}
