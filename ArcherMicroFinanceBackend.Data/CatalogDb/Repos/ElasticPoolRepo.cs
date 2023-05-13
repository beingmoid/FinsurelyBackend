


using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.CatalogDb;

namespace PanoramaBackend.Data.CatalogDb.Repos
{
    public class ElasticPoolRepo : EFRepository<ElasticPools, int>, IElasticPoolRepo
    {
        public ElasticPoolRepo(CatalogDbContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IElasticPoolRepo : IEFRepository<ElasticPools, int>
    {

    }
}
