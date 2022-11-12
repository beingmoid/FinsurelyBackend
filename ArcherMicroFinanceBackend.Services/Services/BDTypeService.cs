using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
