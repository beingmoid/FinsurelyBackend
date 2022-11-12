


using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramBackend.Data.CatalogDb;

namespace PanoramBackend.Data.CatalogDb.Repos
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
