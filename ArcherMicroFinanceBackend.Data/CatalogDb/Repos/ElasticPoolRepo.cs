


using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramBackend.Data.CatalogDb;

namespace PanoramBackend.Data.CatalogDb.Repos
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
