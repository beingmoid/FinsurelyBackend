using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class AccountsMappingService : BaseService<AccountsMapping, int>, IAccountsMappingService
    {
        public AccountsMappingService(RequestScope scopeContext, IAccountsMappingRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IAccountsMappingService : IBaseService<AccountsMapping, int>
    {

    }
}
