using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Repositories
{
    public class CompanyRepository : EFRepository<Company,int>, ICompanyRepository
    {
        public CompanyRepository(AIBContext requestScope):base(requestScope)
        {

        }
    }
    public interface ICompanyRepository : IEFRepository<Company, int>
    {

    }
}
