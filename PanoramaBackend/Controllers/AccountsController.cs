using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using static NukesLab.Core.Common.Constants;
using NukesLab.Core.Api;
using PanoramaBackend.Services;
using PanoramBackend.Data;
using EFCore.BulkExtensions;
using Nest;
using NPOI.SS.Formula.Functions;

namespace PanoramaBackend.Api.Controllers
{

    public class AccountsController : BaseController<Accounts, int>
    {
        private readonly IAccountsService _service;

        private readonly ILedgerEntriesService _ledgerService;
        private ITransactionService _transactionService;
        private readonly AMFContext _context;

        public AccountsController(RequestScope requestScope, IAccountsService service,
            ITransactionService transactionService,
            AMFContext context,
            ILedgerEntriesService ledgerService)
            : base(requestScope, service)
        {
            _service = service;
            _ledgerService = ledgerService;
            _transactionService = transactionService;
            _context = context;
        }

        [HttpGet("GetPaginated")]
        public async Task<BaseResponse> GetPaginated(int page, int pageSize)
        {

            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            var result = (await _service.Get(x => x.Include(x => x.AccountDetailType).ThenInclude(x => x.AccountType)
            .Include(x => x.CreditLedgarEntries)
            .Include(x => x.DebitLedgarEntries)
            ));
            var total= result.Count() / pageSize + (result.Count() % pageSize > 0 ? 1 : 0);

            var skips = result.Skip((page-1)*pageSize).Take(pageSize).ToList();
            foreach (var item in skips)
            {
                if (item.DebitLedgarEntries.Count > 0 | item.CreditLedgarEntries.Count > 0)
                {
                    var credit = item.CreditLedgarEntries.Sum(x => x.Amount);
                    var debit = item.DebitLedgarEntries.Sum(x => x.Amount);
                    if (credit > debit)
                    {
                        item.OpeningBalanceEquity = credit - debit;
                    }
                    else
                    {
                        item.OpeningBalanceEquity = debit - credit;
                    }
                    item.isDeleteApplicable = false;
                }
                else
                {

                    item.isDeleteApplicable = true;
                    item.OpeningBalanceEquity = 0;
                }

                if (item.Id == BuiltinAccounts.AccountsPayable || item.Id == BuiltinAccounts.AccountsRecievable ||
                        item.Id == BuiltinAccounts.CashAccount || item.Id == BuiltinAccounts.ExpenseAccount ||
                        item.Id == BuiltinAccounts.SalesAccount || item.Id == BuiltinAccounts.VATPayable
                        || item.Id == BuiltinAccounts.RetainedEarning || item.Id == BuiltinAccounts.OpeningBalanceEquity

                        )
                {
                    item.isDeleteApplicable = false;
                }


            }
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;

            return (constructResponse(new { data =skips, totalRows = result.Count(), totalPages = total }));
        }
        public async override Task<BaseResponse> Get()
        {
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            var result = await _service.Get(x => x.Include(x => x.AccountDetailType).ThenInclude(x => x.AccountType)
            .Include(x => x.CreditLedgarEntries)
            .Include(x => x.DebitLedgarEntries)
            );
            foreach (var item in result)
            {
                if (item.DebitLedgarEntries.Count > 0 | item.CreditLedgarEntries.Count > 0)
                {
                    var credit = item.CreditLedgarEntries.Sum(x => x.Amount);
                    var debit = item.DebitLedgarEntries.Sum(x => x.Amount);
                    if (credit > debit)
                    {
                        item.OpeningBalanceEquity = credit - debit;
                    }
                    else
                    {
                        item.OpeningBalanceEquity = debit - credit;
                    }
                    item.isDeleteApplicable = false;
                }
                else
                {

                    item.isDeleteApplicable = true;
                    item.OpeningBalanceEquity = 0;
                }

                if (item.Id == BuiltinAccounts.AccountsPayable || item.Id == BuiltinAccounts.AccountsRecievable ||
                        item.Id == BuiltinAccounts.CashAccount || item.Id == BuiltinAccounts.ExpenseAccount ||
                        item.Id == BuiltinAccounts.SalesAccount || item.Id == BuiltinAccounts.VATPayable
                        || item.Id == BuiltinAccounts.RetainedEarning || item.Id == BuiltinAccounts.OpeningBalanceEquity

                        )
                {
                    item.isDeleteApplicable = false;
                }


            }
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;

            return (constructResponse(result));
        }
        [HttpGet("GetAssetAccounts")]           
        public async Task<BaseResponse> GetAssetAccounts()
        {
            var result = await _service.Get(x => x.Include(x => x.AccountDetailType).ThenInclude(x => x.AccountType));
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            return (constructResponse(result));
        }
        //public async Task<BaseResponse> AccountsReceivableList()
        //{
        //    var result = (await _service.Get(x => x.AccountDetailTypeId == 1));
        //    OtherConstants.isSuccessful = true;
        //    OtherConstants.messageType = MessageType.Success;
        //    return constructResponse(result);
        //}
        //public async Task<BaseResponse> AccountsPayableList()
        //{
        //    var result = (await _service.Get(x => x.AccountDetailTypeId == 41));
        //    OtherConstants.isSuccessful = true;
        //    OtherConstants.messageType = MessageType.Success;
        //    return constructResponse(result);
        //}

        //public async Task<BaseResponse> ExpenseAccountsList()
        //{
        //    var result = (await _service.Get(x => x.AccountDetailTypeId == 41));
        //    OtherConstants.isSuccessful = true;
        //    OtherConstants.messageType = MessageType.Success;
        //    return constructResponse(result);
        //}


        [HttpGet("AccountStatement/{id}")]
        public async Task<BaseResponse> AccountStatement(int id)
        {
            var transactionIds = (await _ledgerService.Get(x => x.Include(x => x.CreditAccount).Include(x => x.DebitAccount)

            .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.SaleLineItem)
            , x => x.CreditAccountId == id || x.DebitAccountId == id)).GroupBy(x=>x.TransactionId).Select(x => x.Key).ToList();

            var transactions = _context.Set<Transaction>().Include(x => x.LedgarEntries).ThenInclude(x => x.DebitAccount)
            .Include(x => x.LedgarEntries).ThenInclude(x => x.CreditAccount)
        
            .ToList();
            List<dynamic> list = new List<dynamic>();
            List<Transaction> transacList = new List<Transaction>();
            foreach (var item in transactionIds)
            {
                var service = await _transactionService.Get(x => x.Include(x => x.LedgarEntries).ThenInclude(x => x.DebitAccount)
                 .Include(x => x.LedgarEntries).ThenInclude(x => x.CreditAccount).ThenInclude(x=>x.UserDetail)
                 .Include(x=>x.SalesInvoice).ThenInclude(x=>x.SaleLineItem)
                , x => x.Id == item);
                transacList.Add(service.FirstOrDefault());
            }
            decimal balance = 0;
            foreach (var item in transacList)
            {


                Parallel.ForEach(item.LedgarEntries.ToList(), led =>
                {

                    if (led.DebitAccountId != null)
                    {
                        balance += led.Amount;
                    }
                    else
                    {
                        balance += led.Amount;
                    }

                    var obj = new
                    {
                        TransactionId = item.Id,
                        Date = item.TransactionDate,
                        TransactionType = item.TransactionType.ToString(),
                        Name = led.DebitAccountId != null ? led.DebitAccount.Name : led.CreditAccount.Name,
                        Memo = "Payment Transferred",
                       Debit = led.DebitAccountId != null? led.Amount:0,
                        Credit = led.CreditAccountId != null ? led.Amount : 0,
                        Balance = balance

                    };
                    list.Add(obj);



                });
  


     

            }
            OtherConstants.isSuccessful = true;
            return constructResponse(list);
        }
        [HttpGet("JournalLedger")]
        public async Task<BaseResponse> JournalLedger()
        {
            var ledger = (await _ledgerService.Get(x => x.Include(x => x.Transaction)

            .ThenInclude(x => x.SalesInvoice)
            .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.InsuranceCompany)
            .Include(x => x.Transaction).ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.SalesInvoicePerson)
            .Include(x => x.DebitAccount)
            .Include(x => x.CreditAccount)

            )).Take(200).GroupBy(x => x.TransactionId).Select(x => new
            {
                TransactionId = x.Key,
                Entries = x.OrderBy(x => x.TransactionDate)

            }).ToList();
            var list = new Dictionary<int,dynamic>();
            foreach (var item in ledger)
            {
                foreach (var entry in item.Entries)
                {
                    var obj = new
                    {
                        Date = entry.TransactionDate,
                        TransactionType = entry.Transaction.TransactionType.ToString(),
                        No = item.TransactionId,
                        Name = entry.DebitAccount != null ?
                    entry.Transaction.SalesInvoice.SalesInvoicePerson.DisplayNameAs :
                    entry.Transaction.SalesInvoice.InsuranceCompany.DisplayNameAs,
                        Memo = entry.Transaction.SalesInvoice.Notes,
                        Account = entry.DebitAccount != null ? entry.DebitAccount.Name :
                        entry.CreditAccount?.Name,
                        Debit = entry.DebitAccount != null ? entry.Amount : 0,
                        Credit = entry.CreditAccount != null ? entry.Amount : 0


                    };
                    list.Add(item.TransactionId,obj);
                }
            }
            OtherConstants.isSuccessful = true;
            return constructResponse(list);

        }
    }
}
