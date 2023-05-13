using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIB.Data.Repositories
{
    public class SalesRepository : EFRepository<Sales,int>, ISalesRepository
    {
        public SalesRepository(AIBContext requestScope):base(requestScope)
        {

        }
       // public override IQueryable<Sales> Query => base.Query.Include(x => x.Company).Include(x => x.SalesAgent).Include(x => x.VehicleModel).Include(x => x.MotorType).Include(x=>x.Broker);

        public async Task<List<Sales>> GetPaginatedAPI(int page=1,int pageSize=10)
        {
            var items = await Query.Include(x => x.Company).Include(x => x.SalesAgent).Include(x => x.VehicleModel).Include(x => x.MotorType).Include(x => x.Broker).
                Skip(page * pageSize).Take(pageSize).ToListAsync();
            return items;
        }

    }
    public interface ISalesRepository : IEFRepository<Sales, int>
    {
        Task<List<Sales>> GetPaginatedAPI(int page ,int  pageSize);
    }
}
