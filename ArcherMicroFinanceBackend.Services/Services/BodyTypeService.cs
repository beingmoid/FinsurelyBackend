using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class BodyTypeService : BaseService<BodyType, int>, IBodyTypeService
    {
        public BodyTypeService(RequestScope scopeContext, IBodyTypeRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IBodyTypeService : IBaseService<BodyType, int>
    {

    }
}
