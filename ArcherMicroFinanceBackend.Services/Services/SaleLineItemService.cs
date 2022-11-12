using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class SaleLineItemService : BaseService<SaleLineItem, int>, ISaleLineItemService
    {
        public SaleLineItemService(RequestScope scopeContext, ISalesLineItemRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface ISaleLineItemService : IBaseService<SaleLineItem, int>
    {

    }
}
