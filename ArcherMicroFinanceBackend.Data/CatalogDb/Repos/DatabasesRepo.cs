


using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.CatalogDb;

namespace PanoramaBackend.Data.CatalogDb.Repos
{
    public class DatabasesRepo : EFRepository<Databases, int>, IDatabasesRepo
    {
        public DatabasesRepo(CatalogDbContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IDatabasesRepo : IEFRepository<Databases, int>
    {

    }
}
