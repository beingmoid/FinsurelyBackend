using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

using Service = PanoramaBackend.Data.Entities.Service;
namespace PanoramaBackend.Services.Services
{
    public class ServiceService : BaseService<PanoramaBackend.Data.Entities.Service, int>, IServiceService
    {
        public ServiceService(RequestScope scopeContext, IServiceRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IServiceService : IBaseService<PanoramaBackend.Data.Entities.Service, int>
    {

    }
}
