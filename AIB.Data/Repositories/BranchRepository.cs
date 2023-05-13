using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIB.Data.Repositories
{
    public class BranchRepository : EFRepository<Branch, Guid>, IBranchRepository
    {
        public BranchRepository(AIBContext requestScope):base(requestScope)
        {

        }
        public override IQueryable<Branch> Query => base.Query;
    }
    public interface IBranchRepository : IEFRepository<Branch, Guid>
    {

    }
}
