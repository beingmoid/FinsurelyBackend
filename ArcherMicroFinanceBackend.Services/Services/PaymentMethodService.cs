using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class PaymentMethodService : BaseService<PaymentMethod, int>, IPaymentMethodService
    {
        public PaymentMethodService(RequestScope scopeContext, IPaymentMethodRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IPaymentMethodService : IBaseService<PaymentMethod, int>
    {

    }
}
