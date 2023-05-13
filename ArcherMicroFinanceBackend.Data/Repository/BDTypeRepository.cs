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
