using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
