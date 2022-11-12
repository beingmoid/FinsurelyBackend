using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
