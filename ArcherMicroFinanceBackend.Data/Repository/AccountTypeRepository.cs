using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class AccountTypeRepository : EFRepository<AccountType, int>, IAccountTypeRepository
    {
        public AccountTypeRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IAccountTypeRepository : IEFRepository<AccountType, int>
    {

    }
}
