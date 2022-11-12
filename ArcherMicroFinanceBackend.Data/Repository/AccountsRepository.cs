

using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.Cacheable;
namespace PanoramBackend.Data.Repository
{
    public class AccountsRepository : EFRepository<Accounts, int>, IAccountsRepository
    {
        public AccountsRepository(AMFContext requestScope) : base(requestScope)
        {

        }
        protected override IQueryable<Accounts> Query => base.Query.Include(x=>  x.AccountDetailType).ThenInclude(x => x.AccountType)
            .Include(x => x.CreditLedgarEntries)
            .Include(x => x.DebitLedgarEntries).Cacheable(TimeSpan.FromHours(24))
            ;
    }
    public interface IAccountsRepository : IEFRepository<Accounts, int>
    {

    }
}
