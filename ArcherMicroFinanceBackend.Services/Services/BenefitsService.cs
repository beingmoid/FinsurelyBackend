using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class BenefitsService : BaseService<Benefits, int>, IBenefitsService
    {
        public BenefitsService(RequestScope scopeContext, IBenefitsRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IBenefitsService : IBaseService<Benefits, int>
    {

    }
}
