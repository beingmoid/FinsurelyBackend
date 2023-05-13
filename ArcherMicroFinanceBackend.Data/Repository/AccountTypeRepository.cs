using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
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
