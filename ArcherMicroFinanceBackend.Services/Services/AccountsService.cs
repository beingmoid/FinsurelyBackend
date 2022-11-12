using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PanoramaBackend.Services;
using Microsoft
    .EntityFrameworkCore;

namespace PanoramBackend.Services.Services
{
    public class AccountsService : BaseService<Accounts, int>, IAccountsService
    {
        private readonly ITransactionService _transactionService;

        public AccountsService(RequestScope scopeContext, IAccountsRepository repo ,ITransactionService transactionService) : base(scopeContext, repo,x=>x.CreditLedgarEntries,x=>x.DebitLedgarEntries,x=>x.SubAccounts,x=>x.Refunds,x=>x.UserDetail,x=>x.DepositPayments,x=>x.CreditPayment)
        {
            _transactionService = transactionService;
          
        }

        protected override Task WhileInserting(IEnumerable<Accounts> entities)
        {
            //this.AddNavigation(x => x.CreditLedgarEntries, x => x.DebitLedgarEntries);
            //this.AddIncludeExpression(x => x.Include(x => x.DebitLedgarEntries).Include(x => x.CreditLedgarEntries).Include(x => x.AccountDetailType).ThenInclude(x => x.AccountType));
            return base.WhileInserting(entities);
        }
        protected  async override Task OnInserted(IEnumerable<Accounts> entities)
        {
            foreach (var item in entities)
            {
                if (item.OpeningBalanceEquity!=null)

                {
                    var transaction = new Transaction();

                    transaction.Memo = "Opening Balance";
                    transaction.TransactionDate = (DateTime)item.AsOf;
                    LedgarEntries ledgar = new LedgarEntries();
                    ledgar.TransactionDate = (DateTime)item.AsOf;
                    ledgar.DebitAccountId = item.Id;
                    ledgar.Amount = (decimal) item.OpeningBalanceEquity;
                    transaction.LedgarEntries.Add(ledgar);
                    transaction.TransactionType = TransactionTypes.Deposit;
                    LedgarEntries creditEntry = new LedgarEntries();
                    creditEntry.TransactionDate= (DateTime)item.AsOf;
                    creditEntry.Amount = (decimal)item.OpeningBalanceEquity;
                    creditEntry.CreditAccountId = BuiltinAccounts.OpeningBalanceEquity;
                    transaction.LedgarEntries.Add(creditEntry);
                    await _transactionService.Insert(new[] { transaction });

                }
            }
    
        }
        protected override Task WhileUpdating(IEnumerable<Accounts> entities)
        {
            return base.WhileUpdating(entities);
        }


    }
    public interface IAccountsService : IBaseService<Accounts, int>
    {
  
    }
   
}
