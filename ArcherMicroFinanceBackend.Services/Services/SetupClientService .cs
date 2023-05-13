using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;

namespace PanoramaBackend.Services.Services
{
    public class SetupClientService : BaseService<SetupClient, int>, ISetupClientService
    {
        public SetupClientService(RequestScope scopeContext, ISetupClientRepository repo) : base(scopeContext, repo)
        {

        }
    }

    public interface ISetupClientService : IBaseService<SetupClient, int>
    {

    }
}
