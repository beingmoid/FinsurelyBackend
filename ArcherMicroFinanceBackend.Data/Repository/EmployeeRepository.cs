using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramaBackend.Data.Repository
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
