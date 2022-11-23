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
using static NukesLab.Core.Common.Constants;
using System.Globalization;

namespace PanoramBackend.Services.Services
{
    public class LedgerEntriesService : BaseService<LedgarEntries, int>, ILedgerEntriesService
    {
        private readonly IAccountsService _accountsService;

        public LedgerEntriesService(RequestScope scopeContext, IAccountsService accountsService,ILedgerEntriesRepository repo) : base(scopeContext, repo)
        {
            _accountsService = accountsService;
        }

        public async Task<AccountsReceviableReport> GetAccountReceviableStatement(int accountId)
        {
            var ledger = (await this.Get(x => x.Include(x => x.DebitAccount)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.CustomerDetails)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.SaleLineItem).ThenInclude(x => x.Vehicle)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.Service)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.BodyType)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.PolicyType)
            .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.InsuranceType)
             .Include(x => x.Transaction).ThenInclude(x => x.Payment)
             .Include(x=>x.Transaction).ThenInclude(x=>x.Refund).ThenInclude(x => x.Vehicle)
           .Include(x => x.CreditAccount), x => x.Transaction.UserDetailId.Equals(accountId.ToString())
            )).ToList();

            var accountType = (await _accountsService.Get(x => x.Include(x => x.AccountDetailType).ThenInclude(x => x.AccountType), x => x.Id == accountId)).SingleOrDefault();

            var accountsReport = new AccountsReceviableReport();
            bool isCredit = false;
            bool isDebit = false;   
            if (accountType.AccountDetailType.AccountType.Name == "Accounts Payable (A/P)")
            {
                isDebit = true; ;
            }
            else
            {
                isCredit = true;
            }
            List<Statement> statement = new List<Statement>();
            decimal balance = 0;
            foreach (var item in ledger)
            {
                var entry = new Statement();
              

                entry.Date = item.TransactionDate;

                if (item.Transaction.RefundId!=null)
                {
                    entry.Memo = "REFUND TRANSACTION";
                    entry.PolicyNumber = item.Transaction.Refund.PolicyNumber;
                    entry.Vehicle = item.Transaction.Refund.Vehicle.Make;
                }
                if (item.Transaction.PaymentId!=null)
                {
                    if (item.Transaction.Payment.IsPaymentCredit)
                    {
                        entry.Memo = "Payment Sent";
                    }
                    else
                    {
                        entry.Memo = "Payment Recevied";
                    }
                    
                }


                if (item.CreditAccountId != null)
                {
                    if (item.Transaction.SalesInvoice != null)
                    {
                        entry.InvoiceNumber = (int)item.Transaction.SalesInvoice?.Id + 1000;
                        entry.Name = item.Transaction?.SalesInvoice?.CustomerDetails.DisplayNameAs;
                        entry.InvoiceDate = (DateTime)item.Transaction?.SalesInvoice.SalesInvoiceDate;
                        entry.InsuranceType = item.Transaction?.SalesInvoice?.InsuranceType?.Name;

                    }
                    else
                    {
                        entry.InvoiceDate = item.TransactionDate;
                    }

                    entry.Memo = item.Transaction.Memo;
                    entry.TransactionType = Convert.ToInt32(item.Transaction?.TransactionType.Value);
                    var saleLineItem = item.Transaction?.SalesInvoice.SaleLineItem.SingleOrDefault();

                    entry.AccountName = item.CreditAccount.Name;
                    entry.Amount =  isCredit? (-item.Amount):(item.Amount);
                    entry.Credit = isCredit ? (-item.Amount) : (item.Amount);
                    balance += item.Amount;
                    entry.Balance = balance;
                    if (saleLineItem != null)
                    {
                        entry.PolicyNumber = saleLineItem?.PolicyNumber;
                        entry.Vehicle = saleLineItem?.Vehicle?.Make + " | " + saleLineItem?.Vehicle?.Model;
                    }
                    else
                    {
                        entry.PolicyNumber = "";
                        entry.Vehicle = "";
                    }

                }
                else
                {
                    var saleLineItem = item.Transaction?.SalesInvoice.SaleLineItem.SingleOrDefault();
                    
                    entry.AccountName = item.DebitAccount.Name;
                    entry.Amount = isDebit ? (-item.Amount) : (item.Amount);
                    entry.Debit = isDebit ? (-item.Amount) : (item.Amount);
                    balance += item.Amount;
                    entry.Balance = balance;
                    if (saleLineItem != null)
                    {
                        entry.Name = saleLineItem?.SalesInvoice?.CustomerDetails.DisplayNameAs;
                        entry.Bodytype = saleLineItem.SalesInvoice?.BodyType?.Name;
                        entry.PolicyType=saleLineItem.SalesInvoice?.PolicyType?.Name;
                        entry.Service = saleLineItem.SalesInvoice?.Service.Name;
                        entry.InsuranceType=saleLineItem.SalesInvoice.InsuranceType.Name;
                        entry.PolicyNumber = saleLineItem?.PolicyNumber;
                        entry.Vehicle = saleLineItem?.Vehicle?.Make + " | " + saleLineItem?.Vehicle?.Model;
                    }
                    else
                    {
                        entry.PolicyNumber = "";
                        entry.Vehicle = "";
                    }

                    entry.TransactionType = Convert.ToInt32(item.Transaction.TransactionType);
                    entry.Memo = item.Transaction.Memo;

                    entry.TransactionRefNumber = item.Transaction.Payment?.TransactionReferenceNumber;
                
                }

                statement.Add(entry);

            }
            accountsReport.Statement = statement;
            accountsReport.TotalBalance = balance;
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            return accountsReport;
        }

        public async Task<AccountsReceviableReport> GetAccountReceviableStatement(int accountId, DateTime from, DateTime to)
        {
            var ledger = (await this.Get(x => x.Include(x => x.DebitAccount)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.CustomerDetails)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.SaleLineItem).ThenInclude(x => x.Vehicle)
            .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.InsuranceType)
             .Include(x => x.Transaction).ThenInclude(x => x.Payment)
           .Include(x => x.CreditAccount), x => (x.DebitAccountId == accountId || x.CreditAccountId == accountId) && ((x.TransactionDate.Date >= from.Date) && (x.TransactionDate.Date <= to.Date))
            )).ToList();
            var accountsReport = new AccountsReceviableReport();
            List<Statement> statement = new List<Statement>();
            decimal balance = 0;
            foreach (var item in ledger)
            {
                var entry = new Statement();

                entry.Date = item.TransactionDate;




                if (item.DebitAccountId != null)
                {
                    if (item.Transaction.SalesInvoice != null)
                    {
                        entry.InvoiceNumber = (int)item.Transaction.SalesInvoice?.Id + 1000;
                        entry.Name = item.Transaction?.SalesInvoice?.CustomerDetails?.DisplayNameAs;
                        entry.InvoiceDate = (DateTime)item.Transaction?.SalesInvoice.SalesInvoiceDate;
                        entry.InsuranceType = item.Transaction?.SalesInvoice?.InsuranceType?.Name;
                    }
                    else
                    {
                        entry.InvoiceDate = item.Transaction.Payment.PaymentDate;
                    }

                    entry.Memo = item.Transaction.Memo;
                    entry.TransactionType = Convert.ToInt32(item.Transaction.TransactionType.Value);
                    var saleLineItem = item.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault();

                    entry.AccountName = item.DebitAccount.Description;
                    entry.Amount = item.Amount;
                    entry.Debit = item.Amount;
                    balance += item.Amount;
                    entry.Balance = balance;
                    if (saleLineItem != null)
                    {
                        entry.PolicyNumber = saleLineItem?.PolicyNumber;
                        entry.Vehicle = saleLineItem?.Vehicle?.Make + " | " + saleLineItem?.Vehicle?.Model;
                    }
                    else
                    {
                        entry.PolicyNumber = "";
                        entry.Vehicle = "";
                    }

                }
                else
                {
                    entry.AccountName = item.CreditAccount.Description;
                    entry.TransactionType = Convert.ToInt32(item.Transaction.TransactionType);
                    entry.Memo = item.Transaction.Payment.Memo;
                    entry.Credit = item.Amount;
                    entry.TransactionRefNumber = item.Transaction.Payment?.TransactionReferenceNumber;
                    entry.Amount = -item.Amount;
                    balance += item.Amount;
                    entry.Balance = balance;
                }

                statement.Add(entry);

            }
            accountsReport.Statement = statement;
            accountsReport.TotalBalance = balance;
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            return accountsReport;
        }
        public async Task<AccountStatementReport> GetStatement(int accountId)
        {
            var report = new AccountStatementReport();
            var statement = new List<AccountStatement>();
            var data = (await this.Get(x => x.Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice)
            .Include(x => x.Transaction).ThenInclude(x => x.Payment)
              .Include(x => x.Transaction).ThenInclude(x => x.Refund)
              .Include(x => x.CreditAccount)
              .Include(x => x.DebitAccount)
            , x => x.CreditAccountId == accountId || x.DebitAccountId == accountId));
            decimal balance = 0;
            foreach (var item in data)
            {
                var entry = new AccountStatement();
                entry.Date = item.TransactionDate;
                entry.Activity = (int)item.Transaction.TransactionType;
                if (item.DebitAccountId != null)
                {
                    entry.Debit = item.Amount;
                    balance += item.Amount;
                    entry.Balance = balance;
                }
                else
                {
                    entry.Credit = -item.Amount;
                    balance += item.Amount;
                    entry.Balance = balance;
                }
                statement.Add(entry);
            }
            report.EndingBalance = balance;
            report.Statement = statement;
            return report;
        }

        public async Task<AccountsReceviableReport> SearchAndFilter(int accountId, string start, string end, string sortBy)
        {
            var StartDate = DateTime.ParseExact(start, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
            var EndDate = DateTime.ParseExact(end, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

            var ledger = (await this.Get(x => x.Include(x => x.DebitAccount)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.CustomerDetails)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.SaleLineItem).ThenInclude(x => x.Vehicle)
            .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.InsuranceType)
             .Include(x => x.Transaction).ThenInclude(x => x.Payment)
           .Include(x => x.CreditAccount), x => (x.DebitAccountId == accountId || x.CreditAccountId == accountId) && ((x.TransactionDate.Date >= StartDate.Date) && (x.TransactionDate.Date <= EndDate.Date))
            )).ToList();
            var accountsReport = new AccountsReceviableReport();
            var debit_balance = (await this.Get(x => x.DebitAccountId == accountId && x.TransactionDate.Date <= StartDate.Date)).ToList().Sum(x => x.Amount);
            var credit_balance = (await this.Get(x => x.CreditAccountId == accountId && x.TransactionDate.Date <= StartDate.Date)).ToList().Sum(x => x.Amount);

            List<Statement> statement = new List<Statement>();
            decimal balance = debit_balance-credit_balance;
            statement.Add(new Statement() { Balance = balance,Amount=balance });
            foreach (var item in ledger)
            {
                var entry = new Statement();

                entry.Date = item.TransactionDate;




                if (item.DebitAccountId != null)
                {
                    if (item.Transaction.SalesInvoice != null)
                    {
                        entry.InvoiceNumber = (int)item.Transaction.SalesInvoice?.Id + 1000;
                        entry.Name = item.Transaction?.SalesInvoice?.CustomerDetails?.DisplayNameAs;
                        entry.InvoiceDate = (DateTime)item.Transaction?.SalesInvoice.SalesInvoiceDate;
                        entry.InsuranceType = item.Transaction?.SalesInvoice?.InsuranceType?.Name;
                    }
                    else
                    {
                        entry.InvoiceDate = item.Transaction.Payment.PaymentDate;
                    }

                    entry.Memo = item.Transaction.Memo;
                    entry.TransactionType = Convert.ToInt32(item.Transaction.TransactionType.Value);
                    var saleLineItem = item.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault();

                    entry.AccountName = item.DebitAccount.Description;
                    entry.Amount = item.Amount;
                    entry.Debit = item.Amount;
                    balance += item.Amount;
                    entry.Balance = balance;
                    if (saleLineItem != null)
                    {
                        entry.PolicyNumber = saleLineItem?.PolicyNumber;
                        entry.Vehicle = saleLineItem?.Vehicle?.Make + " | " + saleLineItem?.Vehicle?.Model;
                    }
                    else
                    {
                        entry.PolicyNumber = "";
                        entry.Vehicle = "";
                    }

                }
                else
                {
                    entry.AccountName = item.CreditAccount.Description;
                    entry.TransactionType = Convert.ToInt32(item.Transaction.TransactionType);
                    entry.Memo = item.Transaction.Payment.Memo;
                    entry.Credit = item.Amount;
                    entry.TransactionRefNumber = item.Transaction.Payment?.TransactionReferenceNumber;
                    entry.Amount = -item.Amount;
                    balance = item.Amount;
                    entry.Balance = balance;
                }

                statement.Add(entry);

            }
            accountsReport.Statement = statement;
            accountsReport.TotalBalance = balance;
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                accountsReport.Statement.OrderBy(a => a.GetType().GetProperty(sortBy).GetValue(a, null));
            }

            return accountsReport;
        }


        public async Task<AccountsReceviableReport> SearchAndFilter(int accountId, DateTime start, DateTime end, string sortBy)
        {
            var StartDate = start.Date;
            var EndDate = end.Date;

            var ledger = (await this.Get(x => x.Include(x => x.DebitAccount)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.CustomerDetails)
           .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.SaleLineItem).ThenInclude(x => x.Vehicle)
            .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.InsuranceType)
             .Include(x => x.Transaction).ThenInclude(x => x.Payment)
           .Include(x => x.CreditAccount), x => (x.DebitAccountId == accountId || x.CreditAccountId == accountId) && ( ( (x.TransactionDate.Date >= StartDate.Date) && (x.TransactionDate.Date <= EndDate.Date) ) || ( ((x.Transaction.SalesInvoice.SalesInvoiceDate >= StartDate.Date) && (x.Transaction.SalesInvoice.SalesInvoiceDate <= EndDate.Date)  )) ) 
        )).ToList();
            var accountsReport = new AccountsReceviableReport();
            var debit_balance = (await this.Get(x => x.DebitAccountId == accountId && x.TransactionDate.Date <= StartDate.Date)).ToList().Sum(x => x.Amount);
            var credit_balance = (await this.Get(x => x.CreditAccountId == accountId && x.TransactionDate.Date <= StartDate.Date)).ToList().Sum(x => x.Amount);
            
            List<Statement> statement = new List<Statement>();
            decimal balance = debit_balance - credit_balance;
            if (balance>0)
            {
                statement.Add(new Statement() { Date = start.Date.AddDays(-1), InvoiceDate = start.Date.AddDays(-1), InvoiceNumber = 0, Name = "N/A", TransactionType = 5, AccountName = "N/A", PolicyNumber = "N/A", Debit = 0, Credit = 0, InsuranceType = "N/A", Vehicle = "N/A", Memo = "Previous Balance", TransactionRefNumber = "N/A", Balance = balance, Amount = -balance });
            }
        
        foreach (var item in ledger)
        {
            var entry = new Statement();

            entry.Date = item.TransactionDate;




            if (item.DebitAccountId != null)
            {
                if (item.Transaction.SalesInvoice != null)
                {
                    entry.InvoiceNumber = (int)item.Transaction.SalesInvoice?.Id + 1000;
                    entry.Name = item.Transaction?.SalesInvoice?.CustomerDetails?.DisplayNameAs;
                    entry.InvoiceDate = (DateTime)item.Transaction?.SalesInvoice.SalesInvoiceDate;
                    entry.InsuranceType = item.Transaction?.SalesInvoice?.InsuranceType?.Name;
                }
                else
                {
                    entry.InvoiceDate = item.Transaction.TransactionDate;
                }

                entry.Memo = item.Transaction.Memo;
                entry.TransactionType = Convert.ToInt32(item.Transaction.TransactionType.Value);
                var saleLineItem = item.Transaction?.SalesInvoice?.SaleLineItem.SingleOrDefault();

                entry.AccountName = item.DebitAccount.Description;
                entry.Amount = item.Amount;
                entry.Debit = item.Amount;
                balance += item.Amount;
                entry.Balance = balance;
                if (saleLineItem != null)
                {
                    entry.PolicyNumber = saleLineItem?.PolicyNumber;
                    entry.Vehicle = saleLineItem?.Vehicle?.Make + " | " + saleLineItem?.Vehicle?.Model;
                }
                else
                {
                    entry.PolicyNumber = "";
                    entry.Vehicle = "";
                }

            }
            else
            {
                entry.AccountName = item.CreditAccount.Name;
                entry.TransactionType = Convert.ToInt32(item.Transaction.TransactionType);
                entry.Memo = item.Transaction.Payment.Memo;
                entry.Credit = - item.Amount;
                entry.TransactionRefNumber = item.Transaction.Payment?.TransactionReferenceNumber;
                entry.Amount = - item.Amount;
                balance = item.Amount;
                entry.Balance = balance;
            }

            statement.Add(entry);

        }
        accountsReport.Statement = statement;
        accountsReport.TotalBalance = balance;
        OtherConstants.isSuccessful = true;
        OtherConstants.messageType = MessageType.Success;
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            accountsReport.Statement.OrderBy(a => a.GetType().GetProperty(sortBy).GetValue(a, null));
        }

        return accountsReport;
    }

}
public interface ILedgerEntriesService : IBaseService<LedgarEntries, int>
    {
        Task<AccountsReceviableReport> GetAccountReceviableStatement(int accountId);
        Task<AccountsReceviableReport> GetAccountReceviableStatement(int accountId, DateTime from, DateTime to);
        Task<AccountsReceviableReport> SearchAndFilter(int accountId, DateTime start, DateTime end, string sortBy);
        Task<AccountStatementReport> GetStatement(int accountId);
    }
    public class SearchAndFilter
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string sortBy { get; set; }
        public int AccountId { get; set; }
    }
    public class AccountStatementReport
    {
        public decimal EndingBalance { get; set; }
        public List<AccountStatement> Statement { get; set; }
    }
    public class AccountStatement
    {
        public DateTime Date { get; set; }
        public int Activity { get; set; }
        public string Memo { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal Balance { get; set; }
    }
    public class AccountsReceviableReport
    {
        public List<Statement> Statement { get; set; }
        public decimal TotalBalance { get; set; }

    }
    public class Statement
    {
        public DateTime Date { get; set; }
        public int TransactionType { get; set; }
        public int InvoiceNumber { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InsuranceType { get; set; }
        public string TransactionRefNumber { get; set; }
        public string Name { get; set; }
        public string Bodytype { get; set; }
        public string PolicyType { get; set; }
        public string Service { get; set; }
        public string Memo { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string Vehicle { get; set; }
    }
}
