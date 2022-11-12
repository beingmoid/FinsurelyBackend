using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class AccountsDetailTypeRepository : EFRepository<AccountDetailType, int>, IAccountsDetailTypeRepository
    {
        public AccountsDetailTypeRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IAccountsDetailTypeRepository : IEFRepository<AccountDetailType, int>
    {

    }
}
