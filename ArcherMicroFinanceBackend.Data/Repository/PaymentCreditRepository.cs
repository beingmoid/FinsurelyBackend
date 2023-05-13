using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
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
