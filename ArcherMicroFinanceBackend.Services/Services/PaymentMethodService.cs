using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
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
