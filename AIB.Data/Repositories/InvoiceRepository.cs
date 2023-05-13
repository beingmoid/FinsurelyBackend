using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Repositories
{
    public class InvoiceRepository : EFRepository<Invoice,int>, IInvoiceRepository
    {
        public InvoiceRepository(AIBContext requestScope):base(requestScope)
        {

        }
    }
    public interface IInvoiceRepository : IEFRepository<Invoice, int>
    {

    }
}
