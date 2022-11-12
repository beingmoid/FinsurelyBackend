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

namespace PanoramBackend.Services.Services
{
    public class SupplierService : BaseService<UserDetails, int>, ISupplierService
    {
        private readonly ISalesInvoiceRepository _salesInvoiceService;

        private readonly ITransactionService _transactionService;

        public SupplierService(RequestScope scopeContext, ISupplierRepository repo,
            ISalesInvoiceRepository salesInvoiceRepository,
            ITransactionService transactionService) : base(scopeContext, repo)
        {
            _salesInvoiceService = salesInvoiceRepository;
            _transactionService = transactionService;
        }
        protected override Task WhileInserting(IEnumerable<UserDetails> entities)
        {
            foreach (var item in entities)
            {
                item.IsSupplier = true;
            }
            return base.WhileInserting(entities);
        }
        protected async override Task OnInserted(IEnumerable<UserDetails> entities)
        {
            foreach (var item in entities)
            {
                var paymentAndBilling = item.PaymentAndBilling?.FirstOrDefault();
                if (paymentAndBilling?.OpeningBalance != null)
                {
                    item.IsSupplier = true;
                    var transaction = new Transaction();

                    //Creation Of Sales Invoice
                    SalesInvoice sales = new SalesInvoice();
                    sales.SalesInvoiceDate = DateTime.Now;
                    sales.SalesInvoicePersonId = item.Id;
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
                    var creditTransaction = new Transaction();
                    creditTransaction.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    creditTransaction.Memo = "Opening Balance Equity";
                    LedgarEntries creditEntry = new LedgarEntries();
                    creditEntry.TransactionDate = (DateTime)paymentAndBilling?.Asof;
                    creditEntry.Amount = (decimal)paymentAndBilling?.OpeningBalance;
                    creditEntry.CreditAccountId = BuiltinAccounts.SalesAccount;
                    transaction.LedgarEntries.Add(creditEntry);
                    await _transactionService.Insert(new[] { transaction });

                }
            }
        }
        protected override Task WhileUpdating(IEnumerable<UserDetails> entities)
        {
            foreach (var item in entities)
            {
                item.IsSupplier = true;
            }
            return base.WhileUpdating(entities);
        }
    }
    public interface ISupplierService : IBaseService<UserDetails, int>
    {

    }
}
