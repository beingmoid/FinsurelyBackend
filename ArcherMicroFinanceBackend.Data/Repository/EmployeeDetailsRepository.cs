using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class EmployeeDetailsRepository : EFRepository<EmploymentDetails, int>, IEmployeeDetailsRepository
    { 
    
        public EmployeeDetailsRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IEmployeeDetailsRepository : IEFRepository<EmploymentDetails, int>
    {

    }
}
