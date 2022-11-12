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
    public class BDTypeRepository : EFRepository<BDType, int>, IBDTypeRepositoryy
    {
        public BDTypeRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IBDTypeRepositoryy : IEFRepository<BDType, int>
    {

    }

}
