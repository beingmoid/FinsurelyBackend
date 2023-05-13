using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AIB.Data.Repositories
{
    public class TransactionRepository : EFRepository<Transaction,int>, ITransactionRepository
    {
        public TransactionRepository(AIBContext requestScope):base(requestScope)
        {

        }

        //public override IQueryable<Transaction> Query => base.Query.Include(x => x.Agent).Include(x => x.Bank).Include(x=>x.Company).Include(x=>x.Sales);

        public async Task<List<Transaction>> GetPaginatedAPI( bool isBroker, bool isAgent, bool isBank,int page = 1, int pageSize = 10)
        {

           
            if (isBroker)
            {
                var items = await Query.Include(x => x.Agent).Include(x => x.Bank).Include(x => x.Company).Include(x => x.Sales).Where(x=>x.AgentId==null && x.CompanyId != null).
                Skip((page-1) * pageSize).Take(pageSize).ToListAsync();
                return items;
            }
            else if(isAgent)
            {
                var items = await Query.Include(x => x.Agent).Include(x => x.Bank).Include(x => x.Company).Include(x => x.Sales).Where(x => x.CompanyId == null && x.AgentId != null).
               Skip((page-1) * pageSize).Take(pageSize).ToListAsync();
                return items;
            }
            else
            {
                var items = await Query.Include(x => x.Agent).Include(x => x.Bank).Include(x => x.Company).Include(x => x.Sales).Where(x => x.BankId != null).
                  Skip((page-1) * pageSize).Take(pageSize).ToListAsync();
                return items;
            }
        
        }

        public async Task<List<Transaction>> GetRefundTransactions(int page, int pageSize)
        {
            var items = await Query.Include(x => x.Agent)
    
                .Include(x => x.Company)
                .Include(x=>x.MotorType)
                .Include(x=>x.VehicleModel)
                .Where(x => x.isRefund==true).
                 Skip((page - 1) * pageSize).Take(pageSize).
            ToListAsync();
            return items;
        }
    }
    public interface ITransactionRepository : IEFRepository<Transaction, int>
    {
        Task<List<Transaction>> GetPaginatedAPI(bool isBroker, bool isAgent, bool isBank, int page, int pageSize);
        Task<List<Transaction>> GetRefundTransactions(int page, int pageSize);
    }
}
