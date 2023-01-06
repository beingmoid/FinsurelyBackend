using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static NukesLab.Core.Common.Constants;
using PanoramBackend.Data.Repository;
using PanoramBackend.Data;
using System.Collections.Generic;
using PanoramaBackend.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using FluentExcel;

namespace PanoramaBackend.Api.Controllers
{

    public class InsuranceCompanyController : BaseController<UserDetails, int>
    {
        private readonly IInsuranceCompanyService _service;
        private readonly IComissionRateService _comissionRateService;
        private readonly ILedgerEntriesService _entriesService;
        private readonly AMFContext _context;
        private readonly IWebHostEnvironment _env;

        public InsuranceCompanyController(RequestScope requestScope, IInsuranceCompanyService service,
            ILedgerEntriesService entriesService,
            IComissionRateService comissionRateService,
            IWebHostEnvironment env,
            AMFContext context)
            : base(requestScope, service)
        {
            _service = service;
            _comissionRateService = comissionRateService;
            _entriesService = entriesService;
            _context = context;
            _env = env;
        }
        public async override Task<BaseResponse> Get()
        {
            return constructResponse(await _service.GetCompaniesWithBalance());
        }
        public async override Task<BaseResponse> Get(int id)
        {
            var userDetail = (await _service.Get(x =>
              x.Include(x => x.Addresses)
              .Include(x => x.ComissionRates)
             .Include(x => x.PaymentAndBilling)
               .ThenInclude(x => x.Terms)
             .Include(x => x.PaymentAndBilling)
              .ThenInclude(x => x.PreferredPaymentMethod)
              .Include(x => x.UserDetail)
              .Include(x => x.InsuranceCompanyInvoices).ThenInclude(x => x.Transactions),
              x => x.Id == id

             )).SingleOrDefault();
            // userDetail.ComissionRates.RemoveAll(x => !x.IsActive);



            OtherConstants.isSuccessful = true;
            return constructResponse(userDetail);

        }

        [HttpGet("GetBalance")]
        public async Task<BaseResponse> GetBalance([FromQuery] int id)
        {
            var balance = await _service.GetBalance(id);
            return constructResponse(new { balance = balance });
        }
        [HttpGet("GetComissionRates")]
        public async Task<BaseResponse> GetComissionRates(int id, bool isTpl)
        {
            if (isTpl)
            {
                var comission = (await _comissionRateService.Get(x => x.UserDetailId == id && x.IsTpl && x.IsActive)).SingleOrDefault();
                return constructResponse(comission);
            }
            else
            {
                var comission = (await _comissionRateService.Get(x => x.UserDetailId == id && x.IsNonTpl && x.IsActive)).SingleOrDefault();
                return constructResponse(comission);
            }

        }
        [HttpGet("GetCurrentAccountStatement")]
        public async Task<BaseResponse> GetCurrentAccountStatement(int companyId)
        {
            var agent = (await _service.Get(x => x.Id == companyId)).SingleOrDefault();
            var Debit_Balance = (await _entriesService.Get(x => x.DebitAccountId == agent.DefaultAccountId)).Sum(x => x.Amount);
            var Credit_Balance = (await _entriesService.Get(x => x.CreditAccountId == agent.DefaultAccountId)).Sum(x => x.Amount);

            if (Debit_Balance > Credit_Balance)
            {
                var OpenBalance = Debit_Balance - Credit_Balance;
                var AgentOpenBalance = new CurrentAgentStatementDTO()
                {
                    OpenBalance = OpenBalance
                };
                return constructResponse(AgentOpenBalance);
            }
            else
            {
                var OpenBalance = Credit_Balance - Debit_Balance;
                var AgentOpenBalance = new CurrentAgentStatementDTO()
                {
                    OpenBalance = OpenBalance
                };
                return constructResponse(AgentOpenBalance);
            }


        }

        [HttpPost("GetInsuranceCompanyStatementAsync")]
        public async Task<BaseResponse> GetInsuranceCompanyStatementAsync([FromQuery] PaginationParams<int> @params)
        {
            List<AgentStatementDTO> accountStatement = new List<AgentStatementDTO>();

            PageConfig page = new PageConfig();

            int count = ((@params.ItemsPerPage > 10) || (@params.Page > 1)) ? @params.ItemsPerPage : 0;
            #region Includes/Joins
            var broker = _context.Set<UserDetails>().AsNoTracking().Include(x => x.Accounts)
                          .ThenInclude(x => x.CreditLedgarEntries)
                          .Include(x => x.Accounts)
                          .ThenInclude(x => x.DebitLedgarEntries)
                                .SingleOrDefault(x => x.IsInsuranceCompany == true && x.Id == @params.Id);

            var query = _context.Set<LedgarEntries>().Include(x => x.Transaction)
           .ThenInclude(x => x.SalesInvoice)
                       .ThenInclude(x => x.SaleLineItem)
                           .ThenInclude(x => x.Vehicle)

           .Include(x => x.Transaction)
                  .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.InsuranceType)
       .Include(x => x.Transaction)
                  .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.Branch)


       .Include(x => x.Transaction)
                  .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.BodyType)

                  .Include(x => x.Transaction)
                  .ThenInclude(x => x.Payment)
                  .Include(x => x.Transaction)
                  .ThenInclude(x => x.Refund)
                      .Include(x => x.Transaction)
                      .ThenInclude(x => x.Refund)
                             
                                              .Include(x => x.Transaction)
                                                   .ThenInclude(x => x.Refund)
                                                          .ThenInclude(x => x.Vehicle)
                   .Include(x => x.Transaction)
                                                   .ThenInclude(x => x.Refund)
                                 
                                                          .Include(x => x.Transaction)
                                                   .ThenInclude(x => x.Refund)
                  
                                                                    .Include(x => x.Transaction)
                                                   .ThenInclude(x => x.Refund)

                                                   .ThenInclude(x => x.InsuranceType).AsNoTracking();
            #endregion
            List<LedgarEntries> ledgers = new List<LedgarEntries>();

            #region SearchFilters
            if (broker == null)
            {
                return constructResponse(NotFound());
            }
            //only date
            // 1 1 0
            else if (@params.from != null && @params.to != null && @params.SearchQuery == null)
            {
                #region Query
                ledgers = await query.Where(
            x =>

            (x.CreditAccountId == broker.DefaultAccountId || x.DebitAccountId == broker.DefaultAccountId)

            &&
            (
            (x.TransactionDate.Date >= @params.from.ToDateTime().Date)
            &&
            (x.TransactionDate.Date <= @params.to.ToDateTime().Date)
            )

          ).ToListAsync();

                #endregion
            }
            // date & search query
            //1 1 1
            else if (@params.from != null && @params.to != null && @params.SearchQuery != null)
            {
                var searchString = @params.SearchQuery ?? "";

                #region QUERY
                ledgers = await query.Where(
        x =>

        (x.CreditAccountId == broker.DefaultAccountId || x.DebitAccountId == broker.DefaultAccountId)

        &&
        (
        (x.TransactionDate.Date >= @params.from.ToDateTime().Date)
        &&
        (x.TransactionDate.Date <= @params.to.ToDateTime().Date)
        )
        && (
(

           ((x.Transaction.SalesInvoice.CustomerName.Contains(searchString) || (x.Transaction.Refund.CustomerName.Contains(searchString)))
           ||

           ((x.Transaction.SalesInvoice.SaleLineItem.SingleOrDefault().PolicyNumber.Contains(searchString) || ((x.Transaction.Refund.PolicyNumber.Contains(searchString)))))
           )
           )
           )



      ).ToListAsync();
                #endregion

            }
            //Wo case jab no date only search query for custromer or policy
            //0 0 1
            else if (@params.from == null && @params.to == null && @params.SearchQuery != null)
            {

                string searchString = @params.SearchQuery;
                #region Query
                ledgers = await query.Where(
        x =>

        (x.CreditAccountId == broker.DefaultAccountId || x.DebitAccountId == broker.DefaultAccountId)

        &&

        (
(

           ((x.Transaction.SalesInvoice.CustomerName.Contains(searchString) || (x.Transaction.Refund.CustomerName.Contains(searchString)))







           ||


           ((x.Transaction.SalesInvoice.SaleLineItem.SingleOrDefault().PolicyNumber.Contains(searchString) || ((x.Transaction.Refund.PolicyNumber.Contains(searchString)))))
           )
           )
           )


           )





       .ToListAsync();
                #endregion

            }

            else
            {

                #region Query
                ledgers = await query.Where(
    x =>

    (x.CreditAccountId == broker.DefaultAccountId || x.DebitAccountId == broker.DefaultAccountId)




  )
        .OrderBy(x => x.TransactionDate)
                 .ToListAsync();
                #endregion
            }

            #endregion

            #region GroupByWithPrevBalanceCall
            var entries = ledgers.OrderBy(x => x.TransactionDate).GroupBy(x => x.TransactionId).Select(

    x => new
    {
        Key = x.Key,
        Value = x
    }
    ).ToList();
            page.TotalPages = entries.Count() / @params.ItemsPerPage + (entries.Count() % @params.ItemsPerPage > 0 ? 1 : 0);

            var creditValueForDateGreaterThanParamsToDate = broker.Accounts.CreditLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).Sum(x => x.Amount);
            var debitValueForDateGreaterThanParamsToDate = broker.Accounts.DebitLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).Sum(x => x.Amount);
            var creditEntriesForDateGreaterThanParamsToDate = broker.Accounts.CreditLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).ToList();
            var debitEntriesForDateGreaterThanParamsToDate = broker.Accounts.DebitLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).ToList();

            var brokerBalanceForDateOf = (creditValueForDateGreaterThanParamsToDate) + (-debitValueForDateGreaterThanParamsToDate);

            decimal debitBalance = brokerBalanceForDateOf;


            var _value = (@params.Page - 1) * @params.ItemsPerPage;

#endregion
            var list = entries.ToList();

            //Looping out all kinds of transaction relevant to accountId of Broker/Insurance Company
            foreach (var item in list)
            {
                count++;
                var isItemSalesInvoice = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Invoice
                    );
                var isItemPayment = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Payment);
                var isItemRefund = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Refund);
                var isOpeningBalance = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.OpeningBalance);

                AgentStatementDTO debit = new AgentStatementDTO();

                if (isItemSalesInvoice)
                {

                    var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == broker.Accounts.Id);
                    debitBalance += (item.Value.SingleOrDefault(x => x.CreditAccountId == broker.Accounts.Id)?.Amount) ?? 0;

                    debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                    debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                    debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault()?.PolicyNumber;

                    debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                    debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Make + " " + " | " +
                                _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Model;
                    debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;

                    debit.Debit = _actualItem?.Amount;
                    debit.Credit = (_actualItem?.Amount);
                    debit.Debit = 0;
                    debit.Balance = debitBalance;
                }
                else if (isItemPayment)
                {

                    if (item.Value.SingleOrDefault(x => x.DebitAccountId == broker.Accounts.Id) != null)
                    {

                        var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == broker.Accounts.Id);
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Payment;


                        debitBalance -= _actualItem.Amount;
                        debit.Memo = _actualItem.Transaction?.Payment?.Memo;
                        debit.InvoiceDate = _actualItem?.Transaction?.Payment?.PaymentDate;
                        debit.RefNo = _actualItem?.Transaction?.Payment?.TransactionReferenceNumber;
                        debit.Debit = _actualItem?.Amount;
                        debit.Credit = 0;
                        debit.Debit = (_actualItem?.Amount);
                        debit.Balance = debitBalance;

                    }
                    else
                    {
                        var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == broker.Accounts.Id);
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Payment;


                        debitBalance += _actualItem.Amount;
                        debit.Memo = _actualItem.Transaction?.Payment?.Memo;
                        debit.InvoiceDate = _actualItem?.Transaction?.Payment?.PaymentDate;
                        debit.RefNo = _actualItem?.Transaction?.Payment?.TransactionReferenceNumber;
                        debit.Debit = _actualItem?.Amount;
                        debit.Credit = (_actualItem?.Amount);
                        debit.Debit = 0;
                        debit.Balance = debitBalance;
                    }
                }
                else if (isOpeningBalance)
                {
                    var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == broker.Accounts.Id);
                    debit.Num = count;
                    debit.TransactionType = TransactionTypes.OpeningBalance;
                    debit.Memo = "OPENING BALANCE";
                    debit.HasOnlyMemo = true;
                    debit.InvoiceDate = _actualItem.TransactionDate;
                    debitBalance += (_actualItem?.Amount) ?? 0;
                    debit.Credit = debitBalance;
                    debit.Debit = 0;
                    debit.Balance = debitBalance;
                }
                else
                {

                    debit.Num = count;
                    debit.TransactionType = TransactionTypes.Refund;

                    var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == broker.Accounts.Id);
                    debitBalance -= _actualItem.Amount;
                    debit.InvoiceDate = _actualItem?.Transaction?.Refund?.RefundDate;
                    debit.CustomerName = _actualItem?.Transaction?.Refund?.CustomerName;
                    debit.PolicyNumber = _actualItem?.Transaction?.Refund?.PolicyNumber;
                    debit.PolicyType = _actualItem?.Transaction?.Refund?.PolicyType?.Name;
                    //debit.RefNo = _actualItem?.Transaction?.Refund?.;

                    debit.InsuranceType = _actualItem?.Transaction?.Refund?.InsuranceType?.Name;
                    debit.Vehicle = _actualItem?.Transaction?.Refund?.Vehicle?.Make + " " + " | " + // Make 
                                                                                                    //Concating strings
                                _actualItem?.Transaction?.Refund?.Vehicle?.Model; // Model

                    debit.Memo = _actualItem?.Transaction?.Refund?.MessageOnStatement; ;
                    debit.Debit = _actualItem?.Amount;
                    debit.Credit = (_actualItem?.Amount);
                    debit.Debit = 0;
                    debit.Balance = debitBalance;

                }



            }

            if (@params.RequestExcel!=null)
            {
                #region Excel Export Method

                string wwwPath = _env.WebRootPath;
                string contentPath = _env.ContentRootPath;
                string path = Path.Combine(contentPath, $"\\uploads\\{new DateTime().Ticks.ToString()}.xlsx");
                var serverUrl = this.HttpContext.Request.Host.ToString();
                var ransomeNameStr = new Random().Next(DateTime.Now.Second, 10000).ToString() +
                    new DateTime().Ticks.ToString();
                var isHttps = this.HttpContext.Request.IsHttps;

                var serverPath = isHttps ? "https://" : "http://" + serverUrl + $"/uploads/{ransomeNameStr}.xlsx";
                accountStatement.ToExcel(contentPath + $"\\uploads\\{ransomeNameStr}.xlsx");

                page.ExcelFileUrl = serverPath;
                accountStatement = new List<AgentStatementDTO>();
                #endregion
            }

            return constructResponse(null);
        }
    }
}
