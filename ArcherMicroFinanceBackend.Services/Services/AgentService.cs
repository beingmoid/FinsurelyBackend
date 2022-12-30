﻿using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PanoramaBackend.Services;
using PanoramBackend.Data;
using Microsoft.EntityFrameworkCore;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Services
{
    public class PaginationParams<TKey>

    {
        public TKey Id { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public int? BranchId { get; set; }
        public string? SearchQuery { get; set; }
        public bool? RequestExcel { get; set; }
        public bool? RequestPdf { get; set; }

        private int itemsPerPage;
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }

    }
}
namespace PanoramBackend.Services.Services
{

    public class AgentService : BaseService<UserDetails, int>, IAgentService
    {
        private readonly ITransactionRepository _transactionService;
        private ISalesInvoiceRepository _salesInvoiceService;
        private readonly ILedgerEntriesRepository _ledgerRepo;
        private readonly ILedgerEntriesService _ledger;
        private readonly AMFContext _context;

        public AgentService(RequestScope scopeContext, IAgentRepository repo, ITransactionRepository transactionService,
            ILedgerEntriesRepository ledgerRepo,
           ILedgerEntriesService ledger,
        ISalesInvoiceRepository salesInvoiceService,AMFContext context) : base(scopeContext, repo)
        {
            _transactionService = transactionService;
            _salesInvoiceService = salesInvoiceService;
            _ledgerRepo = ledgerRepo;
            _ledger = ledger;
            _context=context;
            base.AddNavigation(x => x.Addresses, x => x.Attachments,x=>x.PaymentAndBilling,x=>x.SalesInvoicePersons,x=>x.Transactions);
        }
        protected override Task WhileInserting(IEnumerable<UserDetails> entities)
        {
            base.AddNavigation(x => x.Addresses, x => x.Attachments, x => x.PaymentAndBilling);
            foreach (var item in entities)
            {
                item.IsAgent = true;

              
            }
            return base.WhileInserting(entities);
        }

        protected  async override Task OnInserted(IEnumerable<UserDetails> entities)
        {
      
            foreach (var item in entities)
            {
                item.IsAgent = true;
                var paymentAndBilling = item.PaymentAndBilling?.FirstOrDefault();
                if (paymentAndBilling?.OpeningBalance!= null)
            {
                var transaction = new Transaction();

                    //Creation Of Sales Invoice
                 SalesInvoice sales = new SalesInvoice();
                sales.SalesInvoiceDate = DateTime.Now;
                sales.SalesInvoicePersonId = item.Id;
                sales.PaymentStatus = PaymentStatus.Unpaid;
                sales.Total = paymentAndBilling.OpeningBalance;
                 await  _salesInvoiceService.Insert(new[] { sales });
                    //Saving Invoice
                 var result =   await _salesInvoiceService.SaveChanges();
                    //Making Transaction
                transaction.Memo = "Opening Balance";
                transaction.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                transaction.UserDetailId = item.Id;
                transaction.SalesInvoiceId = sales.Id;
                transaction.TransactionType = TransactionTypes.OpeningBalance;
                    //Recording Transaction In Ledger
                LedgarEntries ledgar = new LedgarEntries();
                ledgar.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                ledgar.DebitAccountId = item.DefaultAccountId;
                ledgar.Amount = (decimal)paymentAndBilling?.OpeningBalance;
                transaction.LedgarEntries.Add(ledgar);
                LedgarEntries creditEntry = new LedgarEntries();
                creditEntry.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                creditEntry.Amount = (decimal)paymentAndBilling?.OpeningBalance;
                creditEntry.CreditAccountId = BuiltinAccounts.SalesAccount;
                transaction.LedgarEntries.Add(creditEntry);
                await _transactionService.Insert(new[] { transaction });
                    await _transactionService.SaveChanges();

            }
            }
          
        }
        protected override Task WhileUpdating(IEnumerable<UserDetails> entities)
        {
            foreach (var item in entities)
            {
                item.IsAgent = true;
            }
            base.AddNavigation(x => x.Addresses, x => x.Attachments, x => x.PaymentAndBilling);
            return base.WhileUpdating(entities);
        
        }

        protected async override Task OnUpdated(IEnumerable<UserDetails> entities)
        {
            foreach (var item in entities)
            {
                item.IsAgent = true;
                var paymentAndBilling = item.PaymentAndBilling?.FirstOrDefault();
                if (paymentAndBilling?.OpeningBalance != null)
                {
                    var transaction = new Transaction();

                    //Creation Of Sales Invoice
                    SalesInvoice sales = new SalesInvoice();
                    sales.SalesInvoiceDate = DateTime.Now;
                    sales.SalesInvoicePersonId = item.Id;
                    sales.PaymentStatus = PaymentStatus.Unpaid;
                    sales.Total = paymentAndBilling.OpeningBalance;
                    await _salesInvoiceService.Insert(new[] { sales });
                    //Saving Invoice
                    var result = await _salesInvoiceService.SaveChanges();
                    //Making Transaction
                    transaction.Memo = "Opening Balance";
                    transaction.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    transaction.UserDetailId = item.Id;
                    transaction.SalesInvoiceId = sales.Id;
                    transaction.TransactionType = TransactionTypes.Invoice;
                    //Recording Transaction In Ledger
                    LedgarEntries ledgar = new LedgarEntries();
                    ledgar.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    ledgar.DebitAccountId = item.DefaultAccountId;
                    ledgar.Amount = (decimal)paymentAndBilling?.OpeningBalance;
                    transaction.LedgarEntries.Add(ledgar);
                    LedgarEntries creditEntry = new LedgarEntries();
                    creditEntry.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    creditEntry.Amount = (decimal)paymentAndBilling?.OpeningBalance;
                    creditEntry.CreditAccountId = BuiltinAccounts.SalesAccount;
                    transaction.LedgarEntries.Add(creditEntry);
                    await _transactionService.Insert(new[] { transaction });
                    await _transactionService.SaveChanges();

                }
            }

        }
        public async Task<List<UserDetails>> GetAgentsWithBalance()
        {
                
            var companies = (await this.Get()).ToList();
            var ledger = (await _ledger.Get()).ToList();
            foreach (var item in companies) 
            {
                var debit = ledger.Where(x => x.DebitAccountId == item.DefaultAccountId).Sum(x => x.Amount);
                var credit = ledger.Where(x => x.CreditAccountId == item.DefaultAccountId).Sum(x => x.Amount);
                item.OpenBalance = debit + credit;

            }



            return companies.ToList();
        }
        public async Task<PageConfig> GetPaginatedAgentsWithBalance(PaginationParams<int> @params)
        {
            var query = _context.Set<UserDetails>().AsNoTracking();
            var page = new PageConfig();
            var agentCounts = await query.CountAsync();
            page.TotalCount = agentCounts;
            //List<dynamic> _agents =
            var agents =await query.Include(x => x.Accounts)
                                .ThenInclude(x => x.CreditLedgarEntries)
                                .Include(x => x.Accounts)
                                .ThenInclude(x => x.DebitLedgarEntries)
                                      .Where(x => x.IsAgent == true)
                                      .Skip((@params.Page - 1) * @params.ItemsPerPage)
                                      .Take((@params.ItemsPerPage))
                                      .OrderBy(x => x.CreateTime.Value)
                                            .ToListAsync();

            foreach (var item in agents)
            {
                
                var debit = item.Accounts.DebitLedgarEntries.Sum(x => x.Amount);
                var credit = item.Accounts.CreditLedgarEntries.Sum(x => x.Amount);
                item.OpenBalance = debit + (-credit);
                item.Accounts = null;
            }

            page.Data.AddRange(agents);
            OtherConstants.isSuccessful = true;
            return page;

        }
        
       
        public async Task<decimal> GetBalance(int Id)
        {
            var agent = await this.GetOne(Id);
            var ledger = (await _ledger.Get()).ToList();

            return ledger.Where(x => x.DebitAccountId == agent.DefaultAccountId).Sum(x => x.Amount);
        }
    }
    public interface IAgentService : IBaseService<UserDetails, int>
    {
        Task<List<UserDetails>> GetAgentsWithBalance();
        Task<PageConfig> GetPaginatedAgentsWithBalance(PaginationParams<int> @params);
        Task<decimal> GetBalance(int Id);
        
       
    }
    public class AutocompleteResponse
    {
    
        public string ResponseString { get; set; }


    }
    public class PageConfig
    {
        public PageConfig()
        {
            this.Data = new List<dynamic>();
        }
        public string From { get; set; }
        public string To { get; set; }
        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string ExcelFileUrl { get; set; }
        public string PdfFileUrl { get; set; }
        public decimal TotalBalance { get; set; }
        public List<dynamic> Data { get; set; }

    }
    public class AgentStatementDTO
    {
        public int Num { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string BrokerName { get; set; }

        public string PolicyNumber { get; set; }
        public string PolicyType { get; set; }
        public string ServiceType { get; set; }
        public string InsuranceType { get; set; }
        public string TransactionRef { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public string OpeningBalance { get; set; }
        public string Memo { get; set; }
        public bool HasOnlyMemo { get; set; }
        public string Vehicle { get; set; }
        public string BodyType { get; set; }
        public string RefNo { get; set; }
        public dynamic Debit { get; set; }
        public dynamic Credit { get; set; }
        public dynamic Balance { get; set; }

        public PageConfig PageConfig { get; set; }
    }
}
