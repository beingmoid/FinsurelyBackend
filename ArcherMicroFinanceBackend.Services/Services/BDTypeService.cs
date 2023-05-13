using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class BDTypeService : BaseService<BDType, int>, IBDTypeService
    {
        public BDTypeService(RequestScope scopeContext, IBDTypeRepositoryy repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IBDTypeService : IBaseService<BDType, int>
    {

    }
}
