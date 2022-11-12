using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class PaymentRepository : EFRepository<Payment, int>, IPaymentRepository
    {
        public PaymentRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IPaymentRepository : IEFRepository<Payment, int>
    {

    }
}
