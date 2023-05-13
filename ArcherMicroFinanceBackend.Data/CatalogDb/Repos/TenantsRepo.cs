


using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.CatalogDb;

namespace PanoramaBackend.Data.CatalogDb.Repos
{
    public class TenantsRepo : EFRepository<Tenants, byte[]>, ITenantsRepo
    {
        public TenantsRepo(CatalogDbContext requestScope) : base(requestScope)
        {

        }
    }
    public interface ITenantsRepo : IEFRepository<Tenants, byte[]>
    {

    }
}
