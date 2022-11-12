using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramBackend.Data.Repository
{
    public class EmployeeRepository : EFRepository<UserDetails, int>, IEmployeeRepository
    {
        public EmployeeRepository(AMFContext requestScope) : base(requestScope)
        {

        }
        protected override IQueryable<UserDetails> Query => base.Query.Where(x => x.IsEmployee==true);
    }
    public interface IEmployeeRepository : IEFRepository<UserDetails, int>
    {

    }

}
