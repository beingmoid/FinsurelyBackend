


using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.CatalogDb;

namespace PanoramaBackend.Data.CatalogDb.Repos
{
    public class SeverRepo : EFRepository<Servers, int>, ISeverRepo
    {
        public SeverRepo(CatalogDbContext requestScope) : base(requestScope)
        {

        }
    }
    public interface ISeverRepo : IEFRepository<Servers, int>
    {

    }
}
