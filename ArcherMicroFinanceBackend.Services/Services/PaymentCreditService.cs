using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PanoramBackend.Services.Services
{
    public class PaymentCreditService : BaseService<Payment, int>, IPaymentCreditService
    {
        private readonly ITransactionService _transactionService;

        public PaymentCreditService(RequestScope scopeContext, IPaymentRepository repo,ITransactionService transactionService) : base(scopeContext, repo)
        {
            _transactionService = transactionService;
            this.AddNavigation(x => x.Transactions);

        }
        protected override Task WhileInserting(IEnumerable<Payment> entities)
        {
            foreach (var item in entities)
            {

                item.IsPaymentCredit = true;
                item.IsPaymentDebit = false;
            }
            return base.WhileInserting(entities);
        }
        protected override Task WhileUpdating(IEnumerable<Payment> entities)
        {
            foreach (var item in entities)
            {

                item.IsPaymentCredit = true;
                item.IsPaymentDebit = false;
            }
            return base.WhileUpdating(entities);
        }

        protected async override Task OnInserted(IEnumerable<Payment> entities)
        {
            var Id = entities.ElementAt(0).Id;
            var payment = (await this.Get(x => x.Include(x => x.InsuranceCompany), x => x.Id == Id)).SingleOrDefault();
            var transaction = new Transaction();
            transaction.Memo = payment.Memo;
            transaction.TransactionDate = payment.PaymentDate;
            transaction.PaymentId = payment.Id;
            transaction.TransactionType = TransactionTypes.Bill;
            transaction.UserDetailId = payment.InsuranceCompanyId;
            var debitLedger = new LedgarEntries();
            debitLedger.DebitAccountId = payment.CreditAccountId;
            debitLedger.Amount = payment.Amount;
            debitLedger.TransactionDate = payment.PaymentDate;
            transaction.LedgarEntries.Add(debitLedger);
            var creditLedger = new LedgarEntries();
            creditLedger.CreditAccountId = payment.InsuranceCompany.DefaultAccountId; 
            creditLedger.Amount = payment.Amount;
            creditLedger.TransactionDate = payment.PaymentDate;
            transaction.LedgarEntries.Add(creditLedger);
           await _transactionService.Insert(new[] { transaction });
        }
        protected async override Task OnUpdated(IEnumerable<Payment> entities)
        {
            var Id = entities.ElementAt(0).Id;
            var payment = (await this.Get(x => x.Include(x => x.InsuranceCompany), x => x.Id == Id)).SingleOrDefault();
            var transaction = (await _transactionService.Get(x => x.Include(x => x.LedgarEntries), x => x.PaymentId == payment.Id)).SingleOrDefault();
            transaction.Memo = payment.Memo;
            transaction.TransactionDate = payment.PaymentDate;
            transaction.PaymentId = payment.Id;
            transaction.TransactionType = TransactionTypes.Bill;
            transaction.UserDetailId = payment.InsuranceCompanyId;
            var debitLedger = transaction.LedgarEntries.Where(x => x.DebitAccountId != null).SingleOrDefault();
            debitLedger.DebitAccountId = payment.CreditAccountId; 
            debitLedger.Amount = payment.Amount;
            debitLedger.TransactionDate = payment.PaymentDate;
            transaction.LedgarEntries.Add(debitLedger);
            var creditLedger = transaction.LedgarEntries.Where(x => x.CreditAccountId != null).SingleOrDefault();
            creditLedger.CreditAccountId = payment.InsuranceCompany.DefaultAccountId;
            creditLedger.Amount = payment.Amount;
            creditLedger.TransactionDate = payment.PaymentDate;
            transaction.LedgarEntries.Add(creditLedger);
            await _transactionService.Update(transaction.Id, transaction);
        }
    }
    public interface IPaymentCreditService : IBaseService<Payment, int>
    {

    }
}
