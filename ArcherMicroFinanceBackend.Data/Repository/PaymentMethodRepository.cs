using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class PaymentMethodRepository : EFRepository<PaymentMethod, int>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IPaymentMethodRepository : IEFRepository<PaymentMethod, int>
    {

    }
}
