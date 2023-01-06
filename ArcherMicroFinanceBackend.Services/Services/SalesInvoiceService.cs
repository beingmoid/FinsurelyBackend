using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
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
using PanoramBackend.Data;
using static NukesLab.Core.Common.Constants;

namespace PanoramBackend.Services.Services
{
    public class SalesInvoiceService : BaseService<SalesInvoice, int>, ISalesInvoiceService
    {
        private readonly ITransactionService _transactionService;
        private readonly ILedgerEntriesService ledgerEntriesService;
        private readonly ILedgerEntriesRepository ledgerEntriesRepository;
        private readonly ISalesInvoiceRepository _salesRepo;
        private readonly AMFContext _context;

        public SalesInvoiceService(RequestScope scopeContext, ISalesInvoiceRepository salesRepo, AMFContext context, ISalesInvoiceRepository repo, ITransactionService transactionService, ILedgerEntriesService ledgerEntriesService, ILedgerEntriesRepository ledgerEntriesRepository)
            : base(scopeContext, repo)
        {
            _transactionService = transactionService;
            //this.AddNavigation(x => x.Transactions,x=>x.SaleLineItem);
            this.ledgerEntriesService = ledgerEntriesService;
            this.ledgerEntriesRepository = ledgerEntriesRepository;
            _salesRepo = salesRepo;

            _context = context;
        }

        protected async override Task WhileUpdating(IEnumerable<SalesInvoice> entities)
        {
          


         


                

                //await _transactionService.Update(transactionForAgent.Id, transactionForAgent );
                //await _transactionService.Update(transactionForCompany.Id, transactionForCompany);

            }

            //this.AddNavigation(x => x.Transactions, x => x.SaleLineItem);
            //this.AddIncludeExpression(x => x.Include(x => x.SaleLineItem));
        


        protected async override Task WhileDeleting(IEnumerable<SalesInvoice> entities)
        {
            foreach (var item in entities)
            {
                var sales = (await _salesRepo.Get(x =>
                x.Include(x => x.Transactions).ThenInclude(x => x.LedgarEntries),x=>x.Id==item.Id)).SingleOrDefault();
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
                var sales = (await this.Get(x => x.Include(x => x.CustomerDetails).Include(x => x.SalesInvoicePerson).Include(x => x.SaleLineItem).Include(x => x.InsuranceCompany), x => x.Id == item.Id)).SingleOrDefault();
                ////Making Transaction
                var transactionForAgent = new Transaction();
                //transaction.Memo = "Opening Balance";
                transactionForAgent.TransactionDate = item.SalesInvoiceDate;
                transactionForAgent.UserDetailId = item.SalesInvoicePersonId;
                transactionForAgent.SalesInvoiceId = item.Id;
                transactionForAgent.TransactionType = TransactionTypes.Invoice;


                //Recording Transaction In Ledger
                LedgarEntries Debitledgar = new LedgarEntries();
                Debitledgar.TransactionDate = item.SalesInvoiceDate;
                Debitledgar.DebitAccountId = sales.SalesInvoicePerson.DefaultAccountId; //(A/R)
                Debitledgar.Amount = (decimal)sales.SaleLineItem.FirstOrDefault().SalesPrice;
                transactionForAgent.LedgarEntries.Add(Debitledgar);
                var credit_ledger = new LedgarEntries();
                credit_ledger.TransactionDate = item.SalesInvoiceDate;
                credit_ledger.CreditAccountId = BuiltinAccounts.SalesAccount; //(Income)
                credit_ledger.Amount = (decimal)sales.SaleLineItem.FirstOrDefault().SalesPrice;
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

                var cCreditLedger = new LedgarEntries();
                cCreditLedger.TransactionDate = sales.SalesInvoiceDate;
                cCreditLedger.CreditAccountId = sales.InsuranceCompany?.DefaultAccountId; //AP
                cCreditLedger.Amount = ((decimal)sales.SaleLineItem.FirstOrDefault().Net);
                transactionForCompany.LedgarEntries
                    .Add(cCreditLedger);                LedgarEntries Cdebitledger = new LedgarEntries();
                Cdebitledger.TransactionDate = sales.SalesInvoiceDate;
                Cdebitledger.DebitAccountId = BuiltinAccounts.AccountsPayable; //EX
                Cdebitledger.Amount = (decimal)sales.SaleLineItem.FirstOrDefault().Net;
                transactionForCompany.LedgarEntries.Add(Cdebitledger);

                await _transactionService.Insert(new[] { transactionForAgent });
                await _transactionService.Insert(new[] { transactionForCompany });

            }
        }

        public async Task<bool> UpdateAsync(int id, SalesInvoice entity)
        {
            foreach (var item in new[] { entity })
            {
                ////item.IsSupplier = true;
                //item.Total = item.SaleLineItem.Sum(x=>x.Total);
             
                var sales = (await this.Get(x => x.Include(x => x.CustomerDetails).Include(x => x.SalesInvoicePerson).Include(x => x.SaleLineItem).Include(x => x.InsuranceCompany)
        
                .Include(x => x.SalesInvoicePerson).ThenInclude(x => x.Transactions).ThenInclude(x => x.LedgarEntries)
                     .Include(x => x.InsuranceCompany).ThenInclude(x => x.Transactions).ThenInclude(x => x.LedgarEntries)
                , x => x.Id == item.Id)).SingleOrDefault();

                if (item.SalesInvoicePersonId != sales.SalesInvoicePersonId)
                {
                    var transactionForAgent = sales.SalesInvoicePerson.Transactions.FirstOrDefault(x => x.UserDetailId == sales.SalesInvoicePersonId);
                    //transaction.Memo = "Opening Balance";
                    transactionForAgent.TransactionDate = item.SalesInvoiceDate;
                    transactionForAgent.UserDetailId = item.SalesInvoicePersonId;
                    transactionForAgent.SalesInvoiceId = sales.Id;
                    transactionForAgent.TransactionType = TransactionTypes.Invoice;

                    try
                    {
                        var newSaleAgent = (await _context.Set<UserDetails>().AsNoTracking().Where(x => x.Id == item.SalesInvoicePersonId).ToListAsync()).SingleOrDefault();
                        //Recording Transaction In Ledger
                        LedgarEntries Debitledgar = transactionForAgent.LedgarEntries.SingleOrDefault(x => x.DebitAccountId != null);
                        Debitledgar.TransactionDate = sales.SalesInvoiceDate;
                        Debitledgar.DebitAccountId = newSaleAgent.DefaultAccountId; //(A/R)
                        Debitledgar.Amount = (decimal)item.SaleLineItem.FirstOrDefault().SalesPrice;

                        var credit_ledger = transactionForAgent.LedgarEntries.SingleOrDefault(x => x.CreditAccountId != null);
                        credit_ledger.TransactionDate = item.SalesInvoiceDate;
                        credit_ledger.CreditAccountId = BuiltinAccounts.SalesAccount; //(Income)
                        credit_ledger.Amount = (decimal)item.SaleLineItem.FirstOrDefault().SalesPrice;

                        _context.Set<Transaction>().Update(transactionForAgent);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }

                }
                if (item.InsuranceCompanyId != sales.InsuranceCompanyId)
                {
                    var transactionForCompany = sales.InsuranceCompany.Transactions.SingleOrDefault(x => x.UserDetailId == sales.InsuranceCompanyId);

                    transactionForCompany.TransactionDate = item.SalesInvoiceDate;
                    transactionForCompany.UserDetailId = item.InsuranceCompanyId;
                    transactionForCompany.SalesInvoiceId = sales.Id;
                    transactionForCompany.TransactionType = TransactionTypes.InsuranceCredit;

                    var newSaleAgent = await _context.Set<UserDetails>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == item.InsuranceCompanyId);
                    var cCreditLedger = transactionForCompany.LedgarEntries.SingleOrDefault(x => x.CreditAccountId != null);
                    cCreditLedger.TransactionDate = item.SalesInvoiceDate;
                    cCreditLedger.CreditAccountId = item.InsuranceCompany?.DefaultAccountId; //AP
                    cCreditLedger.Amount = ((decimal)item.SaleLineItem.FirstOrDefault().Net);
                    transactionForCompany.LedgarEntries
                        .Add(cCreditLedger);
                    LedgarEntries Cdebitledger = transactionForCompany.LedgarEntries.SingleOrDefault(x => x.DebitAccountId != null);
                    Cdebitledger.TransactionDate = item.SalesInvoiceDate;
                    Cdebitledger.DebitAccountId = BuiltinAccounts.AccountsPayable; //EX
                    Cdebitledger.Amount = (decimal)item.SaleLineItem.FirstOrDefault().Net;
                    _context.Set<Transaction>().Update(transactionForCompany);
                    await _context.SaveChangesAsync();
                }
            }

                var _sales =  _context.Set<SalesInvoice>().SingleOrDefault(x => x.Id == id);

            //this.Map(entity, sales);
            var entry =_context.Entry(_sales);
            entry.CurrentValues.SetValues(entity);
            var result =  _context.SaveChanges() > 0;
            if(result)
            {
//                var Agent = _context.Set<UserDetails>().FirstOrDefault(x=>x.Id==entity.SalesInvoicePersonId);
//                var Broker = _context.Set<UserDetails>().FirstOrDefault(x => x.Id == entity.InsuranceCompanyId);
//                var transaction = await _transactionService.Get(x=>x.Include(x=>x.LedgarEntries),x => x.SalesInvoiceId == entity.Id);
//                var AgentTran = transaction.SingleOrDefault(x => x.TransactionType == TransactionTypes.Invoice)
//;
//                var CompanTran = transaction.SingleOrDefault(x => x.TransactionType == TransactionTypes.InsuranceCredit);

//                var AgentDebitLedgar = AgentTran.LedgarEntries.SingleOrDefault(x => x.DebitAccountId != null);
//                var AgentCreditLedgar = AgentTran.LedgarEntries.SingleOrDefault(x => x.CreditAccountId != null);

//                AgentDebitLedgar.DebitAccountId = Agent.DefaultAccountId;
//                AgentDebitLedgar.Amount = (decimal) entity.SaleLineItem.SingleOrDefault().SalesPrice;
//                AgentCreditLedgar.Amount = (decimal)entity.SaleLineItem.SingleOrDefault().SalesPrice;
       
//                var BrokerDebitLedgar = AgentTran.LedgarEntries.SingleOrDefault(x => x.DebitAccountId != null);

//                var BrokerCreditLedgar = AgentTran.LedgarEntries.SingleOrDefault(x => x.CreditAccountId != null);


//                BrokerCreditLedgar.CreditAccountId = Broker.DefaultAccountId;
//                BrokerCreditLedgar.Amount = (decimal)entity.SaleLineItem.SingleOrDefault().SalesPrice;
//                BrokerDebitLedgar.Amount = (decimal)entity.SaleLineItem.SingleOrDefault().SalesPrice;

//                _context.Set<Transaction>();
//                _context.Update(AgentTran);
//                _context.Update(CompanTran);
//                _context.SaveChanges();
                OtherConstants.isSuccessful = true;
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                return false;
            }


            }
        



        //transactionForAgent.LedgarEntries.Add(taxEntry);








    }
        public interface ISalesInvoiceService : IBaseService<SalesInvoice, int>
        {
             List<string> GetExcelColumnHeader(string path);
         Task <bool> UpdateAsync(int id, SalesInvoice entity);
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


