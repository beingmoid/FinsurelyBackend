using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PanoramBackend.Services.Services
{
    public class ExpenseService : BaseService<Expense, int>, IExpenseService
    {
        private readonly ITransactionService _transactionService;
        private readonly ILedgerEntriesService _ledgerEntriesService;

        public ExpenseService(RequestScope scopeContext, IExpenseRepository repo,
            
            ITransactionService transactionService,
            ILedgerEntriesService ledgerEntriesService
            ) : base(scopeContext, repo)
        {
            _transactionService = transactionService;
            _ledgerEntriesService = ledgerEntriesService;
        }

        protected override async Task OnInserted(IEnumerable<Expense> entities)
        {
         

            foreach (var item in entities)
            {
                var transaction = new Transaction();
                transaction.ExpenseId = item.Id;
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionType = TransactionTypes.Expense;

                var debitLedger = new LedgarEntries();
                debitLedger.TransactionDate = DateTime.Now;
                debitLedger.DebitAccountId = BuiltinAccounts.ExpenseAccount;
                debitLedger.Amount = item.ExpenseAmount;


               var creditLedger = new LedgarEntries();
                creditLedger.TransactionDate = DateTime.Now;
                creditLedger.CreditAccountId = item.AccountId;
                creditLedger.Amount = item.ExpenseAmount;

                transaction.LedgarEntries.Add(debitLedger);
                transaction.LedgarEntries.Add(creditLedger);

                await _transactionService.Insert( new[] { transaction });

            }
            

        }

        protected override async Task WhileUpdating(IEnumerable<Expense> entities)
        {
            var entity = entities.SingleOrDefault();
            var transaction = (await _transactionService.Get(x => x.Include(x => x.LedgarEntries),x=>x.ExpenseId==entity.Id)).SingleOrDefault();
            var ledgers = transaction.LedgarEntries.ToList();

            foreach (var item in ledgers)
            {
                if (item.CreditAccountId !=null)
                {
                    item.CreditAccountId = entities.SingleOrDefault()?.AccountId;
                }
                item.Amount= entities.SingleOrDefault()?.ExpenseAmount?? 0;
            }


        }
        protected override async Task WhileDeleting(IEnumerable<Expense> entities)
        {
            var entity = entities.SingleOrDefault();
            var transaction = (await _transactionService.Get(x => x.Include(x => x.LedgarEntries), x => x.ExpenseId == entity.Id)).SingleOrDefault();
            var ledgers = transaction.LedgarEntries.ToList();
            foreach (var item in ledgers)
            {
                await _ledgerEntriesService.Delete(item.Id);
            }
            await _transactionService.Delete(transaction.Id);
            
        }

    }
    public interface IExpenseService : IBaseService<Expense, int>
    {

    }
}
