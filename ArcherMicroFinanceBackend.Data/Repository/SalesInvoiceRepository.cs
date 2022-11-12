using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PanoramBackend.Data.Repository
{
    public class SalesInvoiceRepository : EFRepository<SalesInvoice, int>, ISalesInvoiceRepository
    {
        private readonly AMFContext _context;

        public SalesInvoiceRepository(AMFContext requestScope) : base(requestScope)
        {
            _context = requestScope;
        }

        public async Task<bool> UpdateAsync(SalesInvoice salesInvoice)
        {
            _context.Set<SalesInvoice>().Update(salesInvoice).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            var res= await _context.SaveChangesAsync() >0;
            return res;
        }
    }
    public interface ISalesInvoiceRepository : IEFRepository<SalesInvoice, int>
    {
        Task<bool> UpdateAsync(SalesInvoice salesInvoice);
    }
}
