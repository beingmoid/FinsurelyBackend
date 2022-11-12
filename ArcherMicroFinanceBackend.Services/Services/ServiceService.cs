using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class ServiceService : BaseService<Service, int>, IServiceService
    {
        public ServiceService(RequestScope scopeContext, IServiceRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IServiceService : IBaseService<Service, int>
    {

    }
}
