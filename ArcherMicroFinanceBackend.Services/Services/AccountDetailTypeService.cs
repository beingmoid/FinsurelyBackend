using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
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
