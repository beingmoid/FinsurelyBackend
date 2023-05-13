using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramaBackend.Data.Repository
{
    public class BranchRepository : EFRepository<Branch, Guid>, IBranchRepository
    {
        public BranchRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IBranchRepository : IEFRepository<Branch, Guid>
    {

    }

}
