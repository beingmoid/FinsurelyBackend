using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PanoramBackend.Services.Services
{
    public class BranchService : BaseService<Branch, int>, IBranchService
    {
        public BranchService(RequestScope scopeContext, IBranchRepository repo) : base(scopeContext, repo)
        {

        }
   
    }
    public interface IBranchService : IBaseService<Branch, int>
    {

    }
}
