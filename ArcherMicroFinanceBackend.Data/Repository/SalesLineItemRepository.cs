using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class SalesLineItemRepository : EFRepository<SaleLineItem, int>, ISalesLineItemRepository
    {
        public SalesLineItemRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface ISalesLineItemRepository : IEFRepository<SaleLineItem, int>
    {

    }
}
