


using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.CatalogDb;

namespace PanoramaBackend.Data.CatalogDb.Repos
{
    public class OrderRepo : EFRepository<Order, int>, IOrderRepo
    {
        public OrderRepo(CatalogDbContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IOrderRepo : IEFRepository<Order, int>
    {

    }
}
