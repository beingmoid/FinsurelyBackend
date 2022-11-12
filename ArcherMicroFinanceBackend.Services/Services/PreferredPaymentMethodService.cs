using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class PreferredPaymentMethodService : BaseService<PreferredPaymentMethod, int>, IPreferredPaymentMethodService
    {
        public PreferredPaymentMethodService(RequestScope scopeContext, IPreferredPaymentMethodRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IPreferredPaymentMethodService : IBaseService<PreferredPaymentMethod, int>
    {

    }
}
