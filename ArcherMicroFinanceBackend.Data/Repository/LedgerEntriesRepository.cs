using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace PanoramBackend.Data.Repository
{
    public class LedgerEntriesRepository : EFRepository<LedgarEntries, int>, ILedgerEntriesRepository
    {
        AMFContext RequestScope;
        public LedgerEntriesRepository(AMFContext requestScope) : base(requestScope)
        {
            RequestScope = requestScope;
        }
        protected override IQueryable<LedgarEntries> Query => base.Query;
        public async Task<IEnumerable<LedgarEntries>> Joins(List<int> arr)
        {
            var result = RequestScope.Set<LedgarEntries>().Where(x=> arr.Contains(x.Id)).ToList();

            return await Task.FromResult(result);
        }

    }
    public interface ILedgerEntriesRepository : IEFRepository<LedgarEntries, int>
    {
        Task<IEnumerable<LedgarEntries>> Joins(List<int> arr );
    }
}
