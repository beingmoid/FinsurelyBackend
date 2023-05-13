using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
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
