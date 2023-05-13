using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PanoramaBackend.Services;
using System.Linq;

using PanoramaBackend.Data;

namespace PanoramaBackend.Services.Services
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
                    //Making Transaction
                    transaction.Memo = "Opening Balance";
                    transaction.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    transaction.UserDetailId = item.Id;
    
                    transaction.TransactionType = TransactionTypes.OpeningBalance;
                    //Recording Transaction In Ledger
                    LedgarEntries ledgar = new LedgarEntries();
                    ledgar.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    ledgar.DebitAccountId = BuiltinAccounts.AccountsPayable;
                    ledgar.Amount = (decimal)paymentAndBilling?.OpeningBalance;
                    transaction.LedgarEntries.Add(ledgar);


                    var creditTransaction = new Transaction();
                    creditTransaction.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    creditTransaction.Memo = "Opening Balance";
                    LedgarEntries creditEntry = new LedgarEntries();
                    creditEntry.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    creditEntry.Amount = (decimal)paymentAndBilling?.OpeningBalance;
                    creditEntry.CreditAccountId = item.DefaultAccountId;;
                    transaction.LedgarEntries.Add(creditEntry);
                    await _transactionService.Insert(new[] { transaction });

                }
            }

        }

        public async Task<List<UserDetails>> GetCompaniesWithBalance()
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


        public async Task<decimal> GetBalance(int Id)
        {
            var companies = await this.GetOne(Id);
            var ledger = (await _ledger.Get()).ToList();

            return ledger.Where(x =>    x.DebitAccountId == companies.DefaultAccountId).Sum(x => x.Amount);
        }

    }
    public interface IInsuranceCompanyService : IBaseService<UserDetails, int>
    {
        Task<List<UserDetails>> GetCompaniesWithBalance();
        Task<decimal> GetBalance(int Id);
    }
}
