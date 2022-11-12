using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
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
