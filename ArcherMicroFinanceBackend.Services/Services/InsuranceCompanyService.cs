using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PanoramaBackend.Services;
using System.Linq;

using PanoramBackend.Data;

namespace PanoramBackend.Services.Services
{
    public class InsuranceCompanyService : BaseService<UserDetails, int>, IInsuranceCompanyService
    {
        private readonly ITransactionService _transactionService;
        private readonly ISalesInvoiceRepository _salesInvoiceService;
        private IComissionRateReposiotory _comissionService;
        private readonly AMFContext _context;
        private readonly IInsuranceCompanyRepository _repo;
        private readonly ILedgerEntriesService _ledger;

        public InsuranceCompanyService(RequestScope scopeContext, IInsuranceCompanyRepository repo, ITransactionService transactionService,
            IComissionRateReposiotory comissionService,
            ILedgerEntriesService ledger,
            ISalesInvoiceRepository salesInvoiceService
            ,AMFContext context) : base(scopeContext, repo)
        {
            _transactionService = transactionService;
            _salesInvoiceService = salesInvoiceService;
            _comissionService = comissionService;
            _context = context;
            _repo = repo;
            _ledger = ledger;
            this.AddNavigation(x => x.ComissionRates, x => x.Attachments, x => x.PaymentAndBilling, x => x.Addresses);
        }



        protected override Task WhileInserting(IEnumerable<UserDetails> entities)
        {
           
            foreach (var item in entities)
            {
                item.IsInsuranceCompany = true;
                if (item.ComissionRates.Count>2)
                {
                    throw new ServiceException(System.Net.HttpStatusCode.BadRequest);
                }
                foreach (var obj in item.ComissionRates)
                {
                    obj.IsActive = true;
                    if (obj.IsTpl)
                        obj.IsNonTpl = false;
                    if (obj.IsNonTpl)
                        obj.IsTpl = false;
                    obj.ActiveDate = DateTime.Now;
                }
            }
            return base.WhileInserting(entities);
        }

        protected async override Task WhileUpdating(IEnumerable<UserDetails> entities)
        {
               
                  var oldrates =await _comissionService.Get(x => x.UserDetailId == entities.ToArray()[0].Id && x.IsActive);
            foreach (var item in oldrates)
            {
                item.IsActive = false;
                
            }
            
            foreach (var item in entities)
            {
                foreach (var rates in item.ComissionRates)
                {
                    rates.IsActive = true;
                }
                item.IsInsuranceCompany = true;
            }
            _context.BulkUpdate<ComissionRate>(oldrates);
            _context.BulkSaveChanges();
                



        }
        protected async override Task OnInserted(IEnumerable<UserDetails> entities)
        {
            foreach (var item in entities)
            {
                var paymentAndBilling = item.PaymentAndBilling?.FirstOrDefault();
                if (paymentAndBilling?.OpeningBalance != null)
                {
                    var transaction = new Transaction();

                    //Creation Of Sales Invoice
                    SalesInvoice sales = new SalesInvoice();
                    sales.SalesInvoiceDate = DateTime.Now;
                    sales.InsuranceCompanyId = item.Id;
                    sales.Total = paymentAndBilling.OpeningBalance;
                    sales.PaymentStatus = PaymentStatus.Unpaid;
                    await _salesInvoiceService.Insert(new[] { sales });
                    //Saving Invoice
                    var result = await _salesInvoiceService.SaveChanges();
                    //Making Transaction
                    transaction.Memo = "Opening Balance";
                    transaction.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    transaction.UserDetailId = item.Id;
                    transaction.SalesInvoiceId = sales.Id;
                    transaction.TransactionType = TransactionTypes.InsuranceCredit;
                    //Recording Transaction In Ledger
                    LedgarEntries ledgar = new LedgarEntries();
                    ledgar.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    ledgar.DebitAccountId = item.DefaultAccountId;
                    ledgar.Amount = (decimal)paymentAndBilling?.OpeningBalance;
                    transaction.LedgarEntries.Add(ledgar);
                    var creditTransaction = new Transaction();
                    creditTransaction.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    creditTransaction.Memo = "Opening Balance";
                    LedgarEntries creditEntry = new LedgarEntries();
                    creditEntry.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    creditEntry.Amount = (decimal)paymentAndBilling?.OpeningBalance;
                    creditEntry.CreditAccountId = BuiltinAccounts.AccountsPayable;
                    transaction.LedgarEntries.Add(creditEntry);
                    await _transactionService.Insert(new[] { transaction });

                }
            }

        }

        public async Task<List<UserDetails>> GetCompaniesWithBalance()
        {
               
            var companies = await _repo.GetCompaniesWithBalance();
            var ledger = (await _ledger.Get()).ToList();
            foreach (var item in companies)
            {
             
                var credit = ledger.Where(x => x.CreditAccountId == item.DefaultAccountId).Sum(x => x.Amount);
                item.OpenBalance = credit;

            } 



                return companies.ToList();
        }
    }
    public interface IInsuranceCompanyService : IBaseService<UserDetails, int>
    {
        Task<List<UserDetails>> GetCompaniesWithBalance();
    }
}
