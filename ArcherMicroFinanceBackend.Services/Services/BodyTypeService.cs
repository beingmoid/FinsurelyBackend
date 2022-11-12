using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
