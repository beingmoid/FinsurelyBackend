using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
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
