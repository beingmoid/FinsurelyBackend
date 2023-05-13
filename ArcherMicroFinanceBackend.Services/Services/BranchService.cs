using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaBackend.Services.Services
{
    public class BranchService : BaseService<Branch, Guid>, IBranchService
    {
        public BranchService(RequestScope scopeContext, IBranchRepository repo) : base(scopeContext, repo)
        {

        }
   
    }
    public interface IBranchService : IBaseService<Branch, Guid>
    {

    }
}
