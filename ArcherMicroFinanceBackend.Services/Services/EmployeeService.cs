using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PanoramBackend.Services.Services
{
    public class EmployeeService : BaseService<UserDetails, int>, IEmployeeService
    {
        public EmployeeService(RequestScope scopeContext, IEmployeeRepository repo) : base(scopeContext, repo)
        {

        }
        protected override Task WhileInserting(IEnumerable<UserDetails> entities)
        {
            foreach (var item in entities)
            {
                item.IsEmployee = true;
                if(item.EmploymentDetails.Count>0)
                {
                    foreach (var emp in item.EmploymentDetails)
                    {
                        emp.EmployeeIsActive = true;
                    }
                }
            }
            return base.WhileInserting(entities);
        }
        protected override Task WhileUpdating(IEnumerable<UserDetails> entities)
        {
            foreach (var item in entities)
            {
                item.IsEmployee = true;
                if (item.EmploymentDetails.Count > 0)
                {
                    foreach (var emp in item.EmploymentDetails)
                    {
                        emp.EmployeeIsActive = true;
                    }
                }
            }
            return base.WhileUpdating(entities);
        }
    }
    public interface IEmployeeService : IBaseService<UserDetails, int>
    {

    }
}
