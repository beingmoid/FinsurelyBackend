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
using PanoramaBackend.Services;
using static NukesLab.Core.Common.Constants;

namespace PanoramBackend.Services.Services
{
    public class RefundService : BaseService<Refund, int>, IRefundService
    {
        private readonly ITransactionService _transactionService;
        private IComissionRateService _comissionRateService;

        public RefundService(RequestScope scopeContext, IRefundRepository repo,ITransactionService transactionService,
            IComissionRateService comissionRateService) : base(scopeContext, repo)
        {
            _transactionService = transactionService;
            _comissionRateService = comissionRateService;

        }
        protected async override Task OnInserted(IEnumerable<Refund> entities)
        {
            var Id = entities.ElementAt(0);
            var refund =  (await this.Get(x => x.Include(x => x.Agent).Include(x=>x.InsuranceCompany))).SingleOrDefault();
             double taxForAgent = ((double)refund.AmountForSalesAgent)/ 1.05; 
            var transactionForAgent = new Transaction();
            transactionForAgent.Memo = refund.MessageOnStatement;
            transactionForAgent.TransactionDate = refund.RefundDate;
            transactionForAgent.TransactionType = TransactionTypes.Refund;
            transactionForAgent.RefundId = refund.Id;
            transactionForAgent.UserDetailId = refund.AgentId;
              var debitLedger = new LedgarEntries();
            debitLedger.DebitAccountId = BuiltinAccounts.SalesAccount; //Debits Income Account
            debitLedger.Amount = refund.AmountForSalesAgent;
            debitLedger.TransactionDate = refund.RefundDate;
            transactionForAgent.LedgarEntries.Add(debitLedger);
            var creditLedger = new LedgarEntries();
            creditLedger.CreditAccountId = refund.Agent.DefaultAccountId; //Credits Receviable Account
            creditLedger.Amount = refund.AmountForSalesAgent;
            creditLedger.TransactionDate = refund.RefundDate;
            transactionForAgent.LedgarEntries.Add(creditLedger);



            var transactionForBroker = new Transaction();
            transactionForBroker.Memo = refund.MessageOnStatement;
            transactionForBroker.TransactionDate = refund.RefundDate;
            transactionForBroker.TransactionType = TransactionTypes.Refund;
            transactionForBroker.RefundId = refund.Id;
            transactionForBroker.UserDetailId = refund.InsuranceCompanyId;
            var BrokerdebitLedger = new LedgarEntries();
            BrokerdebitLedger.DebitAccountId = refund.InsuranceCompany.DefaultAccountId; //Debits Accounts Payable
            BrokerdebitLedger.Amount = - refund.AmountForBroker;
            BrokerdebitLedger.TransactionDate = refund.RefundDate;
            
            transactionForBroker.LedgarEntries.Add(debitLedger);
            var BrokercreditLedger = new LedgarEntries();
            BrokercreditLedger.CreditAccountId = BuiltinAccounts.ExpenseAccount; // Credits Expense
            BrokercreditLedger.Amount = - refund.AmountForBroker; 
            BrokercreditLedger.TransactionDate = refund.RefundDate;
            transactionForBroker.LedgarEntries.Add(BrokercreditLedger);

            var transactionForIncome = new Transaction();
            transactionForIncome.Memo = "Refunded Income as Profit";
            transactionForIncome.TransactionDate = refund.RefundDate;
            transactionForIncome.TransactionType = TransactionTypes.Deposit;
            transactionForIncome.RefundId = refund.Id;
            var IncomeDebitLedger = new LedgarEntries();
            IncomeDebitLedger.DebitAccountId = BuiltinAccounts.SalesAccount; //Sales Income Debits
            IncomeDebitLedger.Amount = refund.AmountForBroker-refund.AmountForSalesAgent;
            IncomeDebitLedger.TransactionDate = refund.RefundDate;
            transactionForIncome.LedgarEntries.Add(IncomeDebitLedger);
            var IncomeCreditLedger = new LedgarEntries();
            IncomeCreditLedger.CreditAccountId = BuiltinAccounts.RefundIncome; //Refund Income Credits
            IncomeCreditLedger.Amount = refund.AmountForBroker - refund.AmountForSalesAgent;
            IncomeCreditLedger.TransactionDate = refund.RefundDate;
            transactionForIncome.LedgarEntries.Add(IncomeCreditLedger);

 




            await _transactionService.Insert(new[] { transactionForAgent });
            await _transactionService.Insert(new[] { transactionForBroker });
            await _transactionService.Insert(new[] { transactionForIncome });


        }

        protected async override Task OnUpdated(IEnumerable<Refund> entities)
        {
            var Id = entities.ElementAt(0);
            var refund = (await this.Get(x => x.Include(x => x.Agent).Include(x=>x.InsuranceCompany).Include(x=>x.Transactions).ThenInclude(x=>x.LedgarEntries))).SingleOrDefault();
            var transactions = refund.Transactions.ToList();
            var transactionForAgent = (transactions.Where(x => x.UserDetailId == refund.AgentId)).SingleOrDefault();

            transactionForAgent.Memo = refund.MessageOnStatement;
            transactionForAgent.TransactionDate = refund.RefundDate;
            transactionForAgent.TransactionType = TransactionTypes.Refund;
            transactionForAgent.RefundId = refund.Id;
            var creditLedger = transactionForAgent.LedgarEntries.Where(x => x.CreditAccountId != null).SingleOrDefault();
            creditLedger.CreditAccountId = refund.Agent.DefaultAccountId; //Credits Receviable Account
            creditLedger.Amount = refund.AmountForSalesAgent;
            creditLedger.TransactionDate = refund.RefundDate;
            transactionForAgent.LedgarEntries.Add(creditLedger);
            var debitLedger = transactionForAgent.LedgarEntries.Where(x => x.DebitAccountId == BuiltinAccounts.SalesAccount).SingleOrDefault();
            debitLedger.DebitAccountId = BuiltinAccounts.SalesAccount; //Debits Income Account
            debitLedger.Amount = refund.AmountForSalesAgent;
            debitLedger.TransactionDate = refund.RefundDate;
            transactionForAgent.LedgarEntries.Add(debitLedger);
            var transactionForBroker= transactions.Where(x=>x.UserDetailId==refund.InsuranceCompanyId).SingleOrDefault();
            var BrokerdebitLedger = transactionForBroker.LedgarEntries.SingleOrDefault(x => x.DebitAccountId != null);
            BrokerdebitLedger.DebitAccountId = refund.InsuranceCompany.DefaultAccountId; //Debits Accounts Payable
            BrokerdebitLedger.Amount = -refund.AmountForBroker;
            BrokerdebitLedger.TransactionDate = refund.RefundDate;
            transactionForBroker.LedgarEntries.Add(debitLedger);
            var BrokercreditLedger = transactionForBroker.LedgarEntries.SingleOrDefault(x => x.CreditAccountId == BuiltinAccounts.ExpenseAccount);
            BrokercreditLedger.CreditAccountId = BuiltinAccounts.ExpenseAccount; // Credits Expense
            BrokercreditLedger.Amount = - refund.AmountForBroker;
            BrokercreditLedger.TransactionDate = refund.RefundDate;
            transactionForBroker.LedgarEntries.Add(BrokercreditLedger);

            var transactionForIncome = transactions.Where(x => x.UserDetailId == null).SingleOrDefault();
            transactionForIncome.Memo = "Refunded Income as Profit";
            transactionForIncome.TransactionDate = refund.RefundDate;
            transactionForIncome.TransactionType = TransactionTypes.Deposit;
            transactionForIncome.RefundId = refund.Id;
            var IncomeDebitLedger = transactionForIncome.LedgarEntries.SingleOrDefault(x => x.DebitAccountId == BuiltinAccounts.SalesAccount);
           IncomeDebitLedger.DebitAccountId = BuiltinAccounts.SalesAccount; //Sales Income Debits
            IncomeDebitLedger.Amount = - (refund.AmountForBroker - refund.AmountForSalesAgent);
            IncomeDebitLedger.TransactionDate = refund.RefundDate;
            transactionForIncome.LedgarEntries.Add(IncomeDebitLedger);
            var IncomeCreditLedger = transactionForIncome.LedgarEntries.SingleOrDefault(x => x.CreditAccountId == BuiltinAccounts.RefundIncome);
            IncomeCreditLedger.CreditAccountId = BuiltinAccounts.RefundIncome; //Refund Income Credits
            IncomeCreditLedger.Amount = (refund.AmountForBroker - refund.AmountForSalesAgent);
            IncomeCreditLedger.TransactionDate = refund.RefundDate;
            transactionForIncome.LedgarEntries.Add(IncomeCreditLedger);

            await _transactionService.Update(transactionForAgent.Id,transactionForAgent);
            await _transactionService.Update(transactionForBroker.Id, transactionForBroker);
            await _transactionService.Update(transactionForIncome.Id, transactionForIncome);
        }

        public async Task<List<Refund>> GetRefundPayments(DateTime from, DateTime to)
        {

            if (from.Date == DateTime.MinValue && to.Date == DateTime.MinValue)
            {
                var refunds = (await this.Get(x => (x.RefundDate.Date >= from.Date && x.RefundDate.Date <= to.Date)));
                OtherConstants.isSuccessful = true;
                return refunds.ToList();
             
            }
            else
            {
                var refunds = (await this.Get());
                OtherConstants.isSuccessful = true;
                return refunds.ToList();
            }


        }
    }
    public interface IRefundService : IBaseService<Refund, int>
    {
        Task<List<Refund>> GetRefundPayments(DateTime from, DateTime to);
    }
}
