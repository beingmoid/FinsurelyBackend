using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class InsuranceTypeService : BaseService<InsuranceType, int>, IInsuranceTypeService
    {
        public InsuranceTypeService(RequestScope scopeContext, IInsuranceTypeRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IInsuranceTypeService : IBaseService<InsuranceType, int>
    {

    }
}
