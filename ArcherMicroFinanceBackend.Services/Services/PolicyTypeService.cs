using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class PolicyTypeService : BaseService<PolicyType, int>, IPolicyTypeService
    {
        public PolicyTypeService(RequestScope scopeContext, IPolicyTypeRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IPolicyTypeService : IBaseService<PolicyType, int>
    {

    }
}
