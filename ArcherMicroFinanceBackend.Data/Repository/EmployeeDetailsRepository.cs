using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
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
