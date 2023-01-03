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
        private readonly IAgentService _agentService;
        private readonly ITransactionService _transactionService;
        private IComissionRateService _comissionRateService;
        private readonly IInsuranceCompanyService _insuranceCompanyService;
        private readonly ILedgerEntriesService _ledgerEntriesService;

        public RefundService(RequestScope scopeContext, IRefundRepository repo,ITransactionService transactionService,
            IAgentService agentService,
            ILedgerEntriesService ledgerEntriesService,
            IInsuranceCompanyService insuranceCompanyService,
            IComissionRateService comissionRateService) : base(scopeContext, repo)
        {
            _agentService = agentService;
            _transactionService = transactionService;
            _comissionRateService = comissionRateService;
            _insuranceCompanyService = insuranceCompanyService;
            _ledgerEntriesService = ledgerEntriesService;
        }
        protected async override Task OnInserted(IEnumerable<Refund> entities)
        {        ////Broker Account Debit
                 ////Agent Account Credit 
                 ////Income Account Credit

            var refund = entities.ElementAt(0);
            var agent = (await _agentService.Get(x => x.Id == refund.AgentId)).SingleOrDefault();
            var insuranceCompany = (await _insuranceCompanyService.Get(x => x.Id == refund.InsuranceCompanyId)).SingleOrDefault();
            var transaction = new Transaction();
            transaction.Memo = refund.MessageOnStatement;
            transaction.TransactionDate = refund.RefundDate;
            transaction.TransactionType = TransactionTypes.Refund;
            transaction.RefundId = refund.Id;


            var debitLedger = new LedgarEntries();
            debitLedger.DebitAccountId = insuranceCompany.DefaultAccountId; //Debits Broker Payable Account
            debitLedger.Amount = refund.AmountForBroker;
            debitLedger.TransactionDate = refund.RefundDate;
            transaction.LedgarEntries.Add(debitLedger);

            var creditLedgerForAgent = new LedgarEntries();
            creditLedgerForAgent.CreditAccountId = agent.DefaultAccountId; //Credits Agent Receviable Account
            creditLedgerForAgent.Amount = refund.AmountForSalesAgent;
            creditLedgerForAgent.TransactionDate = refund.RefundDate;
            transaction.LedgarEntries.Add(creditLedgerForAgent);

            var differenceAmount = refund.AmountForBroker - refund.AmountForSalesAgent;
            if (differenceAmount > 0)
            {
                var creditLedgerForIncomeAccount = new LedgarEntries();
                creditLedgerForIncomeAccount.CreditAccountId = BuiltinAccounts.RefundIncome; // Credits Refund Income Account
                creditLedgerForIncomeAccount.Amount = differenceAmount;
                creditLedgerForIncomeAccount.TransactionDate = refund.RefundDate;
                transaction.LedgarEntries.Add(creditLedgerForIncomeAccount);
            }
            else if (differenceAmount < 0)
            {
                var creditLedgerForIncomeAccount = new LedgarEntries();
                creditLedgerForIncomeAccount.DebitAccountId = BuiltinAccounts.RefundIncome; // Incase of loss Debits Income Account
                creditLedgerForIncomeAccount.Amount = differenceAmount;
                creditLedgerForIncomeAccount.TransactionDate = refund.RefundDate;
                transaction.LedgarEntries.Add(creditLedgerForIncomeAccount);
            }

            await _transactionService.Insert(new[] { transaction });



            ////Broker Debit
            ////Credit Debit 
            ////Credit Income Account 

            //  var debitLedger = new LedgarEntries();
            //debitLedger.DebitAccountId = BuiltinAccounts.SalesAccount; //Debits Income Account
            //debitLedger.Amount = refund.AmountForSalesAgent;
            //debitLedger.TransactionDate = refund.RefundDate;
            //transactionForAgent.LedgarEntries.Add(debitLedger);
            //var creditLedger = new LedgarEntries();
            //creditLedger.CreditAccountId = agent.DefaultAccountId; //Credits Receviable Account
            //creditLedger.Amount = refund.AmountForSalesAgent;
            //creditLedger.TransactionDate = refund.RefundDate;
            //transactionForAgent.LedgarEntries.Add(creditLedger);

            //var transactionForBroker = new Transaction();
            //transactionForBroker.Memo = refund.MessageOnStatement;
            //transactionForBroker.TransactionDate = refund.RefundDate;
            //transactionForBroker.TransactionType = TransactionTypes.Refund;
            //transactionForBroker.RefundId = refund.Id;
            //transactionForBroker.UserDetailId = refund.InsuranceCompanyId;
            //var BrokerdebitLedger = new LedgarEntries();
            //BrokerdebitLedger.DebitAccountId = insuranceCompany.DefaultAccountId; //Debits Accounts Payable
            //BrokerdebitLedger.Amount =  refund.AmountForBroker;
            //BrokerdebitLedger.TransactionDate = refund.RefundDate;

            //transactionForBroker.LedgarEntries.Add(debitLedger);
            //var BrokercreditLedger = new LedgarEntries();
            //BrokercreditLedger.CreditAccountId = BuiltinAccounts.AccountsPayable; // Credits Expense
            //BrokercreditLedger.Amount =  refund.AmountForBroker; 
            //BrokercreditLedger.TransactionDate = refund.RefundDate;
            //transactionForBroker.LedgarEntries.Add(BrokercreditLedger);

            //var transactionForIncome = new Transaction();
            //transactionForIncome.Memo = "Refunded Income as Profit";
            //transactionForIncome.TransactionDate = refund.RefundDate;
            //transactionForIncome.TransactionType = TransactionTypes.Deposit;
            //transactionForIncome.RefundId = refund.Id;
            //var IncomeDebitLedger = new LedgarEntries();
            //IncomeDebitLedger.DebitAccountId = BuiltinAccounts.SalesAccount; //Sales Income Debits
            //IncomeDebitLedger.Amount = refund.AmountForBroker-refund.AmountForSalesAgent;
            //IncomeDebitLedger.TransactionDate = refund.RefundDate;
            //transactionForIncome.LedgarEntries.Add(IncomeDebitLedger);
            //var IncomeCreditLedger = new LedgarEntries();
            //IncomeCreditLedger.CreditAccountId = refund.AccountId; //Refund Income Credits
            //IncomeCreditLedger.Amount = refund.AmountForBroker - refund.AmountForSalesAgent;
            //IncomeCreditLedger.TransactionDate = refund.RefundDate;
            //transactionForIncome.LedgarEntries.Add(IncomeCreditLedger);






            //await _transactionService.Insert(new[] { transactionForAgent });
            //await _transactionService.Insert(new[] { transactionForBroker });
            //await _transactionService.Insert(new[] { transactionForIncome });


        }

        protected async override Task OnUpdated(IEnumerable<Refund> entities)
        {
            var _refund = entities.ElementAt(0);
            var refund = (await this.Get(x => x.Include(x => x.Agent).Include(x => x.InsuranceCompany).Include(x => x.Transactions).ThenInclude(x => x.LedgarEntries),
                    //where
                    x => x.Id == _refund.Id
               )).SingleOrDefault();
            var agent = refund.Agent;
            var _insuranceCompany = refund.InsuranceCompany;

            var transaction = refund.Transactions.SingleOrDefault();
            transaction.Memo = _refund.MessageOnStatement;
            transaction.TransactionDate = _refund.RefundDate;
            var debitLedger = transaction.LedgarEntries.SingleOrDefault(x => x.DebitAccountId == _insuranceCompany.DefaultAccountId);
            debitLedger.DebitAccountId = _refund.InsuranceCompanyId; //Debits Broker Payable Account
            debitLedger.Amount = _refund.AmountForBroker;
            debitLedger.TransactionDate = _refund.RefundDate;
  

            var creditLedgerForAgent = transaction.LedgarEntries.SingleOrDefault(x => x.CreditAccountId == agent.DefaultAccountId);
            creditLedgerForAgent.CreditAccountId = _refund.AgentId; //Credits Agent Receviable Account
            creditLedgerForAgent.Amount = _refund.AmountForSalesAgent;
            creditLedgerForAgent.TransactionDate = _refund.RefundDate;
            // transaction.LedgarEntries.Add(creditLedgerForAgent);
            var oldDifferenceAmount = refund.AmountForBroker - refund.AmountForSalesAgent;
            var differenceAmount = _refund.AmountForBroker - _refund.AmountForSalesAgent;

            if (oldDifferenceAmount>0 && differenceAmount<0) //When Old Entry was in profit but updated one is in loss
            {
                var creditLedgerForIncomeAccount = transaction.LedgarEntries.SingleOrDefault(x => x.CreditAccountId == BuiltinAccounts.RefundIncome);
                creditLedgerForIncomeAccount.DebitAccountId = BuiltinAccounts.RefundIncome; // Incase of loss Debits Any Income Account
                creditLedgerForIncomeAccount.Amount = differenceAmount;
                creditLedgerForIncomeAccount.TransactionDate = _refund.RefundDate;
                transaction.LedgarEntries.Add(creditLedgerForIncomeAccount);
          
            }
            else //When Old Entry was in loss and new entry is in profit
            {
                var creditLedgerForIncomeAccount = transaction.LedgarEntries.SingleOrDefault(x => x.CreditAccountId == BuiltinAccounts.RefundIncome);
                creditLedgerForIncomeAccount.CreditAccountId = BuiltinAccounts.RefundIncome; // Incase of profit Credits Any Cash Account
                creditLedgerForIncomeAccount.Amount = differenceAmount;
                creditLedgerForIncomeAccount.TransactionDate = _refund.RefundDate;
                transaction.LedgarEntries.Add(creditLedgerForIncomeAccount);
            }
           

            await _transactionService.Insert(new[] { transaction });
        }

        protected async override Task WhileDeleting(IEnumerable<Refund> entities)
        {
            var _refund = entities.ElementAt(0);
            var refund = (await this.Get(x => x.Include(x => x.Agent).Include(x => x.InsuranceCompany).Include(x => x.Transactions).ThenInclude(x => x.LedgarEntries),
                    //where
                    x => x.Id == _refund.Id
               )).SingleOrDefault();
            var agent = refund.Agent;
            var _insuranceCompany = refund.InsuranceCompany;

            var transaction = refund.Transactions.SingleOrDefault();

            foreach (var item in transaction.LedgarEntries)
            {
                await _ledgerEntriesService.Delete(item.Id);
            }
            await _transactionService.Delete(transaction.Id);
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
