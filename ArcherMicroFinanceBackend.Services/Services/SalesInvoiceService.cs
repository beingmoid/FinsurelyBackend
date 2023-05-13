using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PanoramaBackend.Services;
using System.IO;
using System.Net;
using OfficeOpenXml;
using PanoramaBackend.Data;
using static NukesLab.Core.Common.Constants;
using Z.EntityFramework.Plus;
using Microsoft.Data.SqlClient;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Stripe;

namespace PanoramaBackend.Services.Services
{
    public class SalesInvoiceService : BaseService<SalesInvoice, int>, ISalesInvoiceService, IDisposable
    {
        private readonly ITransactionService _transactionService;
        private readonly ILedgerEntriesService ledgerEntriesService;
        private readonly ILedgerEntriesRepository ledgerEntriesRepository;
        private readonly ISalesInvoiceRepository _salesRepo;
        private readonly IAgentService _agentService;
        private readonly IInsuranceCompanyService _insuranceCompanyService;
        private readonly AMFContext _context;

        public SalesInvoiceService(RequestScope scopeContext, ISalesInvoiceRepository salesRepo,

            AMFContext context, ISalesInvoiceRepository repo, ITransactionService transactionService,
            ILedgerEntriesService ledgerEntriesService,
            ILedgerEntriesRepository ledgerEntriesRepository,
            IInsuranceCompanyService insuranceCompanyService,
            IAgentService agentService)
            : base(scopeContext, repo)
        {
            _transactionService = transactionService;
            //this.AddNavigation(x => x.Transactions,x=>x.SaleLineItem);
            this.ledgerEntriesService = ledgerEntriesService;
            this.ledgerEntriesRepository = ledgerEntriesRepository;
            _salesRepo = salesRepo;
            _agentService = agentService;
            _insuranceCompanyService = insuranceCompanyService;
            _context = context;
        }





        protected async override Task WhileDeleting(IEnumerable<SalesInvoice> entities)
        {
            foreach (var item in entities)
            {
                var sales = (await _salesRepo.Get(x =>
                x.Include(x => x.Transactions).ThenInclude(x => x.LedgarEntries), x => x.Id == item.Id)).SingleOrDefault();
                var transactions = sales.Transactions;
                foreach (var tran in transactions)
                {

                    foreach (var led in tran.LedgarEntries)
                    {
                        await ledgerEntriesService.Delete(led.Id);
                    }
                    await _transactionService.Delete(item.Id);
                }
            }

        }
        public List<string> GetExcelColumnHeader(string path)
        {
            var client = new WebClient();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            String url = path;
            var fullPath = Path.GetTempFileName();
            client.DownloadFile(url, fullPath);
            var fileInfo = new FileInfo(fullPath);
            using (ExcelPackage pck = new ExcelPackage(fileInfo))
            {
                var sheet = pck.Workbook.Worksheets[0];
                var columns = sheet.GetHeaderColumns();
                return columns.ToList();

            }
        }

        protected async override Task OnInserted(IEnumerable<SalesInvoice> entities)
        {


            foreach (var item in entities)
            {
                ////item.IsSupplier = true;
                //item.Total = item.SaleLineItem.Sum(x=>x.Total);
                var sales = (await this.Get(x => x.Include(x => x.CustomerDetails).Include(x => x.SalesInvoicePerson).Include(x => x.InsuranceCompany), x => x.Id == item.Id)).SingleOrDefault();
                ////Making Transaction
                var transactionForAgent = new Transaction();
                //transaction.Memo = "Opening Balance";
                transactionForAgent.TransactionDate = item.SalesInvoiceDate;
                transactionForAgent.UserDetailId = item.SalesInvoicePersonId;
                transactionForAgent.SalesInvoiceId = item.Id;
                transactionForAgent.TransactionType = TransactionTypes.Invoice;
                transactionForAgent.BranchId = item.BranchId;

                //Recording Transaction In Ledger
                LedgarEntries Debitledgar = new LedgarEntries();
                Debitledgar.TransactionDate = item.SalesInvoiceDate;
                Debitledgar.DebitAccountId = sales.SalesInvoicePerson.DefaultAccountId; //(A/R)
                Debitledgar.Amount = (decimal)sales.SalesPrice;
                transactionForAgent.LedgarEntries.Add(Debitledgar);
                var credit_ledger = new LedgarEntries();
                credit_ledger.TransactionDate = item.SalesInvoiceDate;
                credit_ledger.CreditAccountId = BuiltinAccounts.SalesAccount; //(Income)
                credit_ledger.Amount = (decimal)sales.SalesPrice;
                transactionForAgent.LedgarEntries.Add(credit_ledger);
                //var taxEntry = new LedgarEntries();
                //taxEntry.TransactionDate = item.SalesInvoiceDate;
                //taxEntry.CreditAccountId = BuiltinAccounts.VATPayable;
                //taxEntry.Amount = (decimal)item.SaleLineItem.FirstOrDefault().VAT;
                //transactionForAgent.LedgarEntries.Add(taxEntry);
                var transactionForCompany = new Transaction();

                transactionForCompany.TransactionDate = sales.SalesInvoiceDate;
                transactionForCompany.UserDetailId = sales.InsuranceCompanyId;
                transactionForCompany.SalesInvoiceId = sales.Id;
                transactionForCompany.TransactionType = TransactionTypes.InsuranceCredit;
                transactionForCompany.BranchId = item.BranchId;
                var cCreditLedger = new LedgarEntries();
                cCreditLedger.TransactionDate = sales.SalesInvoiceDate;
                cCreditLedger.CreditAccountId = sales.InsuranceCompany?.DefaultAccountId; //AP
                cCreditLedger.Amount = ((decimal)sales.Net);
                transactionForCompany.LedgarEntries
                    .Add(cCreditLedger); LedgarEntries Cdebitledger = new LedgarEntries();
                Cdebitledger.TransactionDate = sales.SalesInvoiceDate;
                Cdebitledger.DebitAccountId = BuiltinAccounts.AccountsPayable; //EX
                Cdebitledger.Amount = (decimal)sales.Net;
                transactionForCompany.LedgarEntries.Add(Cdebitledger);

                await _transactionService.Insert(new[] { transactionForAgent });
                await _transactionService.Insert(new[] { transactionForCompany });

            }
        }

        public async Task<bool> UpdateAsync(int id, SalesInvoice entity)
        {
            var valueBeforeUpdate = _context.Set<SalesInvoice>().AsNoTracking().SingleOrDefault(x => x.Id == id);


            var data = _context.Set<SalesInvoice>()

                       .Include(x => x.SalesInvoicePerson)
                           .ThenInclude(x => x.Transactions)
                           .ThenInclude(x => x.LedgarEntries)
                       .Include(x => x.InsuranceCompany)
                           .ThenInclude(x => x.Transactions)
                           .ThenInclude(x => x.LedgarEntries)

                       .AsNoTracking()

                       .SingleOrDefault(x => x.Id == id);
            UserDetails newAgent = null;
            UserDetails newBroker = null;
            if (valueBeforeUpdate.SalesInvoicePersonId != entity.SalesInvoicePersonId)
            {
                newAgent = await _agentService.GetOne(entity.SalesInvoicePersonId ?? 0);
            }
            if (valueBeforeUpdate.InsuranceCompanyId != entity.InsuranceCompanyId)
            {
                newBroker = await _insuranceCompanyService.GetOne(entity.InsuranceCompanyId ?? 0);
            }
            var AgentTransaction = data.SalesInvoicePerson.Transactions.SingleOrDefault();
            var CompanyTransaction = data.InsuranceCompany.Transactions.SingleOrDefault();



            _context.Entry(entity).State = EntityState.Detached;

            try
            {
                var result = await this.Update(id, entity);

                if (result.Success)
                {

                    var transactions = AgentTransaction;
                    if (newAgent != null)
                    {
              
                        transactions.UserDetailId = newAgent.Id;
                    }
                    transactions.UserDetails = null;
                    transactions.TransactionDate = entity.SalesInvoiceDate;
                    transactions.TransactionType = TransactionTypes.Invoice;
                    transactions.SalesInvoiceId = entity.Id;
                    transactions.BranchId = entity.BranchId;
                    transactions.LedgarEntries.Clear();
                    _context.Entry(transactions).State = EntityState.Detached;
                    var result2 = await _transactionService.Update(transactions.Id, transactions);
                    var agentLedger = _context.Set<LedgarEntries>().AsNoTracking().Where(x => x.TransactionId == transactions.Id).ToList();
                    foreach (var item in agentLedger)
                    {
                        if (item.DebitAccountId != null)
                        {
                            item.TransactionDate = entity.SalesInvoiceDate;
                            if (newAgent != null)
                            {
                                item.DebitAccountId = newAgent.DefaultAccountId; //(A/R)
                                item.DebitAccount = null;
                            }

                            item.Amount = (decimal)entity.SalesPrice;
                        }
                        if (item.CreditAccountId != null)
                        {
                            item.TransactionDate = entity.SalesInvoiceDate;
                            item.Amount = (decimal)entity.SalesPrice;
                        }

                        _context.Entry(item).State = EntityState.Detached;
                        var ledgerResult = await ledgerEntriesService.Update(item.Id, item);
                    }

                    var brokerTransaction = CompanyTransaction;
   
                    if (newBroker != null)
                    {
                       
                        brokerTransaction.UserDetailId = newBroker.Id;
                    }
                    brokerTransaction.UserDetails = null;
                    brokerTransaction.TransactionDate = entity.SalesInvoiceDate;
                    brokerTransaction.TransactionType = TransactionTypes.Invoice;
                    brokerTransaction.SalesInvoiceId = entity.Id;
                    brokerTransaction.BranchId = entity.BranchId;
                    brokerTransaction.LedgarEntries.Clear();
                    _context.Entry(brokerTransaction).State = EntityState.Detached;
                    var result3 = await _transactionService.Update(brokerTransaction.Id, brokerTransaction);
                    var brokerLedger = _context.Set<LedgarEntries>().AsNoTracking().Where(x => x.TransactionId == brokerTransaction.Id).ToList();

                    foreach (var item in brokerLedger)
                    {
                        if (item.CreditAccountId != null)
                        {
                            item.TransactionDate = entity.SalesInvoiceDate;
                            if (newBroker != null)
                            {
                                item.CreditAccountId = newBroker.DefaultAccountId; //(A/R)
                                item.CreditAccount = null;
                            }

                            item.Amount = (decimal)entity.Net;
                        }
                        if (item.DebitAccountId != null)
                        {
                            item.TransactionDate = entity.SalesInvoiceDate;
                            item.Amount = (decimal)entity.Net;
                        }

                        _context.Entry(item).State = EntityState.Detached;
                        var ledgerResult = await ledgerEntriesService.Update(item.Id, item);
                    }



                }
                return result.Success;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await this.GetOne(id) != null))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }




        }









        public void Dispose(object obj)
        {
            GC.SuppressFinalize(obj);

        }


    }
    public interface ISalesInvoiceService : IBaseService<SalesInvoice, int>
    {
        List<string> GetExcelColumnHeader(string path);
        Task<bool> UpdateAsync(int id, SalesInvoice entity);

    }

    public static class ExcelWorksheetExtension
    {
        /// <summary>
        ///     Get Header row with EPPlus. 
        ///     <a href="https://stackoverflow.com/questions/10278101/epplus-reading-column-headers">
        ///         EPPlus Reading Column Headers
        ///     </a>
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns>Array of headers</returns>
        public static string[] GetHeaderColumns(this ExcelWorksheet sheet)
        {
            return sheet.Cells[sheet.Dimension.Start.Row, sheet.Dimension.Start.Column, 1, sheet.Dimension.End.Column]
                .Select(firstRowCell => firstRowCell.Text).ToArray();
        }
    }
}


