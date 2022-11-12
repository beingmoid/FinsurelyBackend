using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using static NukesLab.Core.Common.Constants;

namespace PanoramBackend.Services.Services
{
    public class TransactionService : BaseService<Transaction, int>, ITransactionService
    {
        public TransactionService(RequestScope scopeContext, ITransactionRepository repo) : base(scopeContext, repo,x=>x.LedgarEntries)
        {
            //this.AddNavigation(x => x.LedgarEntries);
        }

       public async Task<List<Transaction>> GetTransferPayments(DateTime from, DateTime to)
        {

            if (from.Date == DateTime.MinValue && to.Date == DateTime.MinValue)
            {
                var transfers = (await this.Get(x => (x.TransactionDate.Date >= from.Date && x.TransactionDate.Date <= to.Date) && x.TransactionType==TransactionTypes.Transfer));
                OtherConstants.isSuccessful = true;
                return transfers.ToList();
            }
            else
            {
                var transfers = (await this.Get(x => x.TransactionType == TransactionTypes.Transfer));
                OtherConstants.isSuccessful = true;
                return transfers.ToList();
            }

           
        }
    }
    public interface ITransactionService : IBaseService<Transaction, int>
    {
        Task<List<Transaction>> GetTransferPayments(DateTime from, DateTime to);
    }
  
}
