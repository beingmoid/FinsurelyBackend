using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PanoramaBackend.Data.Repository
{
    public class InsuranceCompanyRepository : EFRepository<UserDetails, int>, IInsuranceCompanyRepository
    {
        public InsuranceCompanyRepository(AMFContext requestScope) : base(requestScope)
        {

        }

        protected override IQueryable<UserDetails> Query => base.Query.Where(x=>x.IsInsuranceCompany==true);
        
         public async Task<List<UserDetails>> GetCompaniesWithBalance()
        {
            var companies = await this.Get(x => x.Include(x => x.Transactions).ThenInclude(x => x.LedgarEntries)
            , x => x.IsInsuranceCompany == true);
            return companies.ToList();
        }
    }
    public interface IInsuranceCompanyRepository : IEFRepository<UserDetails, int>
    {
        Task<List<UserDetails>> GetCompaniesWithBalance();
    }
}
