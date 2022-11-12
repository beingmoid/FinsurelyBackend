using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class AccountTypeService : BaseService<AccountType, int>, IAccountTypeService
    {
        public AccountTypeService(RequestScope scopeContext, IAccountTypeRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IAccountTypeService : IBaseService<AccountType, int>
    {

    }
}
