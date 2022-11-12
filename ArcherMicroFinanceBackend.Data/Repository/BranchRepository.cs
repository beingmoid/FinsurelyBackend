using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramBackend.Data.Repository
{
    public class BranchRepository : EFRepository<Branch, int>, IBranchRepository
    {
        public BranchRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IBranchRepository : IEFRepository<Branch, int>
    {

    }

}
