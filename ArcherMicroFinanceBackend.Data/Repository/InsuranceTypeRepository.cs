using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class InsuranceTypeRepository : EFRepository<InsuranceType, int>, IInsuranceTypeRepository
    {
        public InsuranceTypeRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IInsuranceTypeRepository : IEFRepository<InsuranceType, int>
    {

    }
}
