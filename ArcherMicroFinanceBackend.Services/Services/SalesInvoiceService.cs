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
using Z.EntityFramework.Plus;
using Microsoft.Data.SqlClient;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Stripe;

namespace PanoramBackend.Services.Services
{
    public class SalesInvoiceService : BaseService<SalesInvoice, int>, ISalesInvoiceService
    {
        private readonly ITransactionService _transactionService;
        private readonly ILedgerEntriesService ledgerEntriesService;
        private readonly ILedgerEntriesRepository ledgerEntriesRepository;
        private readonly ISalesInvoiceRepository _salesRepo;
        private readonly IAgentService _agentService;
        private readonly AMFContext _context;

        public SalesInvoiceService(RequestScope scopeContext, ISalesInvoiceRepository salesRepo, 
            
            AMFContext context, ISalesInvoiceRepository repo, ITransactionService transactionService, 
            ILedgerEntriesService ledgerEntriesService, 
            ILedgerEntriesRepository ledgerEntriesRepository,
            IAgentService agentService)
            : base(scopeContext, repo)
        {
            _transactionService = transactionService;
            //this.AddNavigation(x => x.Transactions,x=>x.SaleLineItem);
            this.ledgerEntriesService = ledgerEntriesService;
            this.ledgerEntriesRepository = ledgerEntriesRepository;
            _salesRepo = salesRepo;
            _agentService = agentService;
            _context = context;
        }

     
          


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
          
            _context.Entry(entity).State = EntityState.Modified;

            foreach (SaleLineItem child in entity.SaleLineItem)
            {
                _context.Entry(child).State = child.Id == 0 ? EntityState.Added : EntityState.Modified;
            }

            try
            {
                return await _context.SaveChangesAsync()>0;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! (await this.GetOne(id)!=null))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            //var dbEntity = await this.Repository.GetOne(id);
            //dbEntity.SaleLineItem.Clear();
            //this.Map(entity, dbEntity);
            //var result = await _salesRepo.UpdateAsync(dbEntity);


            //try
            //{

            // //   var result = await this.Repository.SaveChanges();
            //    if (result)
            //    {

            //        OtherConstants.isSuccessful = true;
            //        return true;
            //    }
            //    else
            //    {
            //        OtherConstants.isSuccessful = false;
            //        return false;
            //    }
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    foreach (var entry in ex.Entries)
            //    {
            //        if (entry.Entity is SalesInvoice)
            //        {
            //            var proposedValues = entry.CurrentValues;
            //            var databaseValues = entry.GetDatabaseValues();

            //            foreach (var property in proposedValues.Properties)
            //            {
            //                var proposedValue = proposedValues[property];
            //                var databaseValue = databaseValues[property];

            //                // TODO: decide which value should be written to database
            //                // proposedValues[property] = <value to be saved>;
            //            }

            //            // Refresh original values to bypass next concurrency check
            //            entry.OriginalValues.SetValues(databaseValues);
            //            await Repository.SaveChanges();
            //        }
            //        else
            //        {
            //            throw new NotSupportedException(
            //                "Don't know how to handle concurrency conflicts for "
            //                + entry.Metadata.Name);

            //        }
            //    }
            //    return false;
            //}
            //catch (UniqueConstraintException ex)
            //{
            //    throw ex;
            //}
            //catch (CannotInsertNullException ex)
            //{
            //    throw ex;
            //}
            //catch (MaxLengthExceededException ex)
            //{
            //    throw ex;
            //}
            //catch (ReferenceConstraintException ex)
            //{
            //    throw ex;
            //}
            //catch (NumericOverflowException ex)
            //{
            //    throw ex;
            //}

            //catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            //{
            //    throw ex;
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}





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


