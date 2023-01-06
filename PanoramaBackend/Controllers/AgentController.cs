using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
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
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Stripe;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using DotLiquid;
using Nest;
using PanoramBackend.Data;
using Elastic.Apm;
using FluentExcel;
using PanoramaBackend.Services;
using Wkhtmltopdf.NetCore;
using Microsoft.AspNetCore.Routing;

namespace PanoramaBackend.Api.Controllers
{

    public class AgentController : BaseController<UserDetails, int>
    {
        private readonly IAgentService _service;
        private readonly ILedgerEntriesService _entriesService;
        private readonly ITransactionService _tranService;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _env;
        private readonly AMFContext _context;
        private readonly IGeneratePdf _generatePDF;
        private readonly IRazorViewToStringRenderer _engine;
        const string Port = "5000";
        public AgentController(RequestScope requestScope, IAgentService service, ILedgerEntriesService entriesService
            , ITransactionService tranService,
            AMFContext context,
            IWebHostEnvironment env,
            IRazorViewToStringRenderer engine,
            IGeneratePdf generatePDF,

            IConverter converter            )
            : base(requestScope, service)
        {
            _service = service;
            _entriesService = entriesService;
            _tranService = tranService;
            _converter = converter;
            _env = env;
            _context = context;
            _generatePDF = generatePDF;
            _engine = engine;

        }



        [HttpPost("SearchAndFilter")]
        public async Task<BaseResponse> SearchAndFilter([FromBody] SearchAndFilter filter, int accountId, string start, string end, string sortBy)
        {

            return constructResponse(await _entriesService.SearchAndFilter(accountId, filter.start, filter.end, filter.sortBy));

        }
        public async override Task<BaseResponse> Get()
        {
            return constructResponse(await _service.GetAgentsWithBalance());
        }

        [HttpGet("GetOne")]
        public async Task<BaseResponse> GetOne(int id)
        {
            var userDetail = (await _service.Get(x =>
              x.Include(x => x.Addresses)
             .Include(x => x.PaymentAndBilling)
               .ThenInclude(x => x.Terms)
             .Include(x => x.PaymentAndBilling)
              .ThenInclude(x => x.PreferredPaymentMethod)
              .Include(x => x.UserDetail)
              .Include(x => x.SalesInvoicePersons).ThenInclude(x => x.Transactions),
              x => x.Id == id
             )).SingleOrDefault();
            OtherConstants.isSuccessful = true;
            return constructResponse(userDetail);

        }

        [HttpGet("GetBalance")]
        public async Task<BaseResponse> GetBalance([FromQuery] int id)
        {
            if (id>0)
            {
                var balance = await _service.GetBalance(id);
                OtherConstants.isSuccessful = true;
                return constructResponse(new { balance = balance });
             
            }
            OtherConstants.isSuccessful = false;
            OtherConstants.responseMsg = "Please use correct param";
            return constructResponse(BadRequest());
         
        }

        private async Task<PageConfig> GetAgentStatementByDate(int agentId , DateTime  dateFrom , DateTime dateTo)
        {
            var agent = (await _service.Get(x => x.Include(x => x.Accounts), x => x.Id == agentId)).SingleOrDefault();

            var ledgers = (await _entriesService.Get(x => x.Include(x => x.Transaction)

            .ThenInclude(x => x.SalesInvoice)
                        .ThenInclude(x => x.SaleLineItem)
                            .ThenInclude(x => x.Vehicle)

            .Include(x => x.Transaction)
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.InsuranceType)
        .Include(x => x.Transaction)
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.Branch)
  
   


        .Include(x => x.Transaction)
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.BodyType)



            , x => x.TransactionDate>=dateFrom && x.TransactionDate<=dateTo && x.Transaction.UserDetailId == agentId)).GroupBy(x => x.TransactionId).Select(

                x => new
                {
                    Key = x.Key,
                    Value = x
                }
                );

            //List<dynamic> accountStatement = new List<dynamic>();
            PageConfig page = new PageConfig();

            return page;

        }

        [AllowAnonymous]
        [HttpPost("GetAgentPaginated")]

        public async Task<BaseResponse> GetAgentPaginated([FromBody] PaginationParams<int> @params)
        {
            List<AgentStatementDTO> accountStatement = new List<AgentStatementDTO>();
     
            PageConfig page = new PageConfig();
            int count = ((@params.ItemsPerPage > 10) || (@params.Page > 1)) ? @params.ItemsPerPage : 0;


            var agent = _context.Set<UserDetails>().AsNoTracking().Include(x => x.Accounts)
                               .ThenInclude(x => x.CreditLedgarEntries)
                               .Include(x => x.Accounts)
                               .ThenInclude(x => x.DebitLedgarEntries)
                                     .SingleOrDefault(x => x.IsAgent == true && x.Id == @params.Id);


            Console.Clear();
    
            Console.WriteLine("Last SQL Execution");
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
                                              
                                                          .Include(x => x.Transaction)
                                                 
                                                                    .Include(x => x.Transaction)
                                                   .ThenInclude(x => x.Refund)

                                                   .ThenInclude(x => x.InsuranceType);







            if (agent == null)
            {

                return constructResponse(NotFound());
            }

            //only date
            // 1 1 0
            else if (@params.from != null && @params.to != null && @params.SearchQuery == null)
            {

                #region Query
                var ledgers = query.Where(
             x =>

             (x.CreditAccountId == agent.DefaultAccountId || x.DebitAccountId == agent.DefaultAccountId)

             &&
             (
             (x.TransactionDate.Date >= @params.from.ToDateTime().Date)
             &&
             (x.TransactionDate.Date <= @params.to.ToDateTime().Date)
             )

           ).ToList();

                #endregion
                #region Excel Export
                if (@params.RequestExcel != null)
                {

                    var groupedList = ledgers.OrderBy(x => x.TransactionDate).GroupBy(x => x.TransactionId).Select(

               x => new
               {
                   Key = x.Key,
                   Value = x
               }
               ).ToList();



                    decimal Balance = (agent.Accounts.DebitLedgarEntries
                        .Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date)
                                    .Sum(x => x.Amount)) + (-(agent.Accounts.CreditLedgarEntries.
                                    Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).
                                    Sum(x => x.Amount)));

                    foreach (var item in groupedList)
                    {
                        count++;
                        var isItemSalesInvoice = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Invoice ||

                        x.Transaction.TransactionType == TransactionTypes.Payment
                            );
                        var isItemPayment = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Payment);
                        var isItemRefund = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Refund);
                        var isOpeningBalance = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.OpeningBalance);

                        AgentStatementDTO debit = new AgentStatementDTO();
                        if (isItemSalesInvoice)
                        {

                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.Invoice;
                            if (item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id) != null)
                            {
                                var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                                Balance += _actualItem.Amount;
                                debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                                debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                                debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault().PolicyNumber;
                      
                                debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                                debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Make + " " + " | " +
                                            _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Model;
                                debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                                debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                                debit.Debit = item?.Value?.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Amount;
                                debit.Credit = 0;
                                debit.Balance = Balance;

                            }
                            else
                            {
                                var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                                Balance -= (item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount) ?? 0;

                                debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                                debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                                debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault()?.PolicyNumber;
                 
                                debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                                debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Make + " " + " | " +
                                            _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Model;
                                debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                                debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                                debit.Debit = _actualItem?.Amount;
                                debit.Credit = (_actualItem?.Amount);
                                debit.Debit = 0;
                                debit.Balance = Balance;

                            }
                        }
                        else if (isItemPayment)
                        {
                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.Payment;

                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            Balance += _actualItem.Amount;
                            debit.Memo = _actualItem.Transaction?.Payment?.Memo;
                            debit.InvoiceDate = _actualItem?.Transaction?.Payment?.PaymentDate;
                            debit.RefNo = _actualItem?.Transaction?.Payment?.TransactionReferenceNumber;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = Balance;

                        }
                        else if (isOpeningBalance)
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.OpeningBalance;
                            debit.Memo = "OPENING BALANCE";
                            debit.HasOnlyMemo = true;
                            debit.InvoiceDate = _actualItem.TransactionDate;
                            Balance += (_actualItem?.Amount) ?? 0;
                            debit.Debit = Balance;
                            debit.Balance = Balance;
                        }
                        else
                        {
                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.Refund;

                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            Balance -= _actualItem.Amount;
                            debit.InvoiceDate = _actualItem?.Transaction?.Refund?.RefundDate;
                            debit.CustomerName = _actualItem?.Transaction?.Refund?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.Refund?.PolicyNumber;
                 

                            debit.InsuranceType = _actualItem?.Transaction?.Refund?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.Refund?.Vehicle?.Make + " " + " | " + // Make 
                                                                                                            //Concating strings
                                        _actualItem?.Transaction?.Refund?.Vehicle?.Model; // Model

                            debit.Memo = _actualItem?.Transaction?.Refund?.MessageOnStatement; ;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = Balance;

                        }






                        accountStatement.Add(debit);

                    }

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
                #endregion
                var entries = ledgers.OrderBy(x => x.TransactionDate).GroupBy(x => x.TransactionId).Select(

                    x => new
                    {
                        Key = x.Key,
                        Value = x
                    }
                    ).ToList();


                page.TotalPages = entries.Count() / @params.ItemsPerPage + (entries.Count() % @params.ItemsPerPage > 0 ? 1 : 0);
                var agentCurrentBalance = agent.Accounts.DebitLedgarEntries.Sum(x => x.Amount) + (-agent.Accounts.CreditLedgarEntries.Sum(x => x.Amount)
);


                var creditValueForDateGreaterThanParamsToDate = agent.Accounts.CreditLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).Sum(x => x.Amount);
                var debitValueForDateGreaterThanParamsToDate = agent.Accounts.DebitLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).Sum(x => x.Amount);
                var creditEntriesForDateGreaterThanParamsToDate = agent.Accounts.CreditLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).ToList();
                var debitEntriesForDateGreaterThanParamsToDate = agent.Accounts.DebitLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).ToList();

                var agentBalanceForDateOf = debitValueForDateGreaterThanParamsToDate + (-creditValueForDateGreaterThanParamsToDate);

                decimal debitBalance = agentBalanceForDateOf;
                // decimal creditBalance = 0;
                var _value = (@params.Page - 1) * @params.ItemsPerPage;


                var list = entries.ToList();
                //var lastItem = list.Last();

                foreach (var item in list)
                {
                    count++;
                    var isItemSalesInvoice = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Invoice ||

                    x.Transaction.TransactionType == TransactionTypes.Payment
                        );
                    var isItemPayment = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Payment);
                    var isItemRefund = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Refund);
                    var isOpeningBalance = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.OpeningBalance);

                    AgentStatementDTO debit = new AgentStatementDTO();
                    if (isItemSalesInvoice)
                    {

                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Invoice;
                        if (item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id) != null)
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                            debitBalance += _actualItem.Amount;
                            debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                            debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault().PolicyNumber;
          
                            debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Make + " " + " | " +
                                        _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Model;
                            debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                            debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                            debit.Debit = item?.Value?.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Amount;
                            debit.Credit = 0;
                            debit.Balance = debitBalance;

                        }
                        else
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            debitBalance -= (item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount) ?? 0;

                            debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                            debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault()?.PolicyNumber;
            
                            debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Make + " " + " | " +
                                        _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Model;
                            debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                            debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = debitBalance;

                        }
                    }
                    else if (isItemPayment)
                    {
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Payment;

                        var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                        debitBalance += _actualItem.Amount;
                        debit.Memo = _actualItem.Transaction?.Payment?.Memo;
                        debit.InvoiceDate = _actualItem?.Transaction?.Payment?.PaymentDate;
                        debit.RefNo = _actualItem?.Transaction?.Payment?.TransactionReferenceNumber;
                        debit.Debit = _actualItem?.Amount;
                        debit.Credit = (_actualItem?.Amount);
                        debit.Debit = 0;
                        debit.Balance = debitBalance;

                    }
                    else if (isOpeningBalance)
                    {
                        var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.OpeningBalance;
                        debit.Memo = "OPENING BALANCE";
                        debit.HasOnlyMemo = true;
                        debit.InvoiceDate = _actualItem.TransactionDate;
                        debitBalance += (_actualItem?.Amount) ?? 0;
                        debit.Debit = debitBalance;
                        debit.Balance = debitBalance;
                    }
                    else
                    {
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Refund;

                        var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                        debitBalance -= _actualItem.Amount;
                        debit.InvoiceDate = _actualItem?.Transaction?.Refund?.RefundDate;
                        debit.CustomerName = _actualItem?.Transaction?.Refund?.CustomerName;
                        debit.PolicyNumber = _actualItem?.Transaction?.Refund?.PolicyNumber;
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



                    if (list.IndexOf(item) == 0)
                    {
                        var entry = new AgentStatementDTO();
                        entry.TransactionType = TransactionTypes.OpeningBalance;
                        entry.Memo = "Opening balance as balance for date before " + @params.from.ToDateTime().Date.ToShortDateString();
                        entry.Balance = agentBalanceForDateOf;
                        entry.Num = count;
                        count++;
                    }


                    accountStatement.Add(debit);

                }

                OtherConstants.isSuccessful = true;
                if (@params.RequestPdf != null)
                {
                    var serverUrl = this.HttpContext.Request.Host.ToString();
                    var statemenmtPDF = new AccountStatementPDF();
                    statemenmtPDF.AccountTRN = agent.Id.ToString() + "/" + agent.CreateTime?.ToBinary().ToString() + "/" + DateTime.Now.Ticks;
                    statemenmtPDF.AgentName = agent.DisplayNameAs;
                    statemenmtPDF.DateFrom = @params.from.ToDateTime().ToShortDateString();
                    statemenmtPDF.DateTo = @params.to.ToDateTime().ToShortDateString();
                    statemenmtPDF.Country = "United Arab Emirates";
                    statemenmtPDF.Emirates = "Dubai";
                    statemenmtPDF.Statement = accountStatement;
                    var render = await _engine.RenderViewToStringAsync("GetAgentPaginated", statemenmtPDF);
                    var doc = new HtmlToPdfDocument()
                    {
                        GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Landscape,
                        PaperSize = PaperKind.A4Plus,
                    },
                        Objects = {
                        new ObjectSettings() {

                            PagesCount = true,
                            HtmlContent = render,
                            LoadSettings =
                            {
                                JSDelay=1000,
                                StopSlowScript=false,

                            },
                            WebSettings = { DefaultEncoding = "utf-8",
                            LoadImages=true,
                            EnableIntelligentShrinking=true,
                            EnableJavascript=true,
                            enablePlugins=true,
                            PrintMediaType=true
                            ,Background=true,

                            },
                            HeaderSettings =
                            {
                                HtmUrl=$"http://{serverUrl}/header?id={agent.Id}&from={ statemenmtPDF.DateFrom }&to={statemenmtPDF.DateTo}&balance={debitBalance}"

                            }

                        }
                    }
                    };
                    byte[] pdf = _converter.Convert(doc);
                    string wwwPath = _env.WebRootPath;
                    string contentPath = _env.ContentRootPath;
                    var ransomeNameStr = new Random().Next(DateTime.Now.Second, 10000).ToString() +
                    new DateTime().Ticks.ToString();

                    var isHttps = this.HttpContext.Request.IsHttps;
                    var serverPath = isHttps ? "https://" : "http://" + serverUrl + $"/uploads/{ransomeNameStr}.pdf";
                    System.IO.File.WriteAllBytes(contentPath + $"\\Uploads\\{ransomeNameStr}.pdf", pdf);

                    page.PdfFileUrl = serverPath;
                }
                page.Data.AddRange(accountStatement.Skip((@params.Page - 1) * @params.ItemsPerPage).Take(@params.ItemsPerPage).ToList());
                return constructResponse(page);




            }

            // date & search query
            //1 1 1
            else if (@params.from != null && @params.to != null && @params.SearchQuery != null)
            {
                var searchString = @params.SearchQuery ?? "";

                #region QUERY
                var ledgers = query.Where(
         x =>

         (x.CreditAccountId == agent.DefaultAccountId || x.DebitAccountId == agent.DefaultAccountId)

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



       ).ToList();
                #endregion
                #region Excel Export
                if (@params.RequestExcel != null)
                {

                    var groupedList = ledgers.OrderBy(x => x.TransactionDate).GroupBy(x => x.TransactionId).Select(

               x => new
               {
                   Key = x.Key,
                   Value = x
               }
               ).ToList();



                    decimal Balance = (agent.Accounts.DebitLedgarEntries
                        .Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date)
                                    .Sum(x => x.Amount)) + (-(agent.Accounts.CreditLedgarEntries.
                                    Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).
                                    Sum(x => x.Amount)));

                    foreach (var item in groupedList)
                    {
                        count++;
                        var isItemSalesInvoice = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Invoice ||

                        x.Transaction.TransactionType == TransactionTypes.Payment
                            );
                        var isItemPayment = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Payment);
                        var isItemRefund = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Refund);
                        var isOpeningBalance = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.OpeningBalance);

                        AgentStatementDTO debit = new AgentStatementDTO();
                        if (isItemSalesInvoice)
                        {

                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.Invoice;
                            if (item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id) != null)
                            {
                                var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                                Balance += _actualItem.Amount;
                                debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                                debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                                debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault().PolicyNumber;
                   

                                debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                                debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Make + " " + " | " +
                                            _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Model;
                                debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                                debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                                debit.Debit = item?.Value?.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Amount;
                                debit.Credit = 0;
                                debit.Balance = Balance;

                            }
                            else
                            {
                                var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                                Balance -= (item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount) ?? 0;

                                debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                                debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                                debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault()?.PolicyNumber;
                
                                debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                                debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Make + " " + " | " +
                                            _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Model;
                                debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                                debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                                debit.Debit = _actualItem?.Amount;
                                debit.Credit = (_actualItem?.Amount);
                                debit.Debit = 0;
                                debit.Balance = Balance;

                            }
                        }
                        else if (isItemPayment)
                        {
                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.Payment;

                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            Balance += _actualItem.Amount;
                            debit.Memo = _actualItem.Transaction?.Payment?.Memo;
                            debit.InvoiceDate = _actualItem?.Transaction?.Payment?.PaymentDate;
                            debit.RefNo = _actualItem?.Transaction?.Payment?.TransactionReferenceNumber;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = Balance;

                        }
                        else if (isOpeningBalance)
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.OpeningBalance;
                            debit.Memo = "OPENING BALANCE";
                            debit.HasOnlyMemo = true;
                            debit.InvoiceDate = _actualItem.TransactionDate;
                            Balance += (_actualItem?.Amount) ?? 0;
                            debit.Debit = Balance;
                            debit.Balance = Balance;
                        }
                        else
                        {
                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.Refund;

                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            Balance -= _actualItem.Amount;
                            debit.InvoiceDate = _actualItem?.Transaction?.Refund?.RefundDate;
                            debit.CustomerName = _actualItem?.Transaction?.Refund?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.Refund?.PolicyNumber;

                            debit.InsuranceType = _actualItem?.Transaction?.Refund?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.Refund?.Vehicle?.Make + " " + " | " + // Make 
                                                                                                            //Concating strings
                                        _actualItem?.Transaction?.Refund?.Vehicle?.Model; // Model

                            debit.Memo = _actualItem?.Transaction?.Refund?.MessageOnStatement; ;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = Balance;

                        }






                        accountStatement.Add(debit);

                    }

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
                #endregion
               
                var entries = ledgers.OrderBy(x => x.TransactionDate).GroupBy(x => x.TransactionId).Select(

                    x => new
                    {
                        Key = x.Key,
                        Value = x
                    }
                    ).ToList();


                page = new PageConfig();
                count = ((@params.ItemsPerPage > 10) || (@params.Page > 1)) ? @params.ItemsPerPage : 0;
                page.TotalPages = entries.Count() / @params.ItemsPerPage + (entries.Count() % @params.ItemsPerPage > 0 ? 1 : 0);
                var creditValueForDateGreaterThanParamsToDate = agent.Accounts.CreditLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).Sum(x => x.Amount);
                var debitValueForDateGreaterThanParamsToDate = agent.Accounts.DebitLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).Sum(x => x.Amount);
                var creditEntriesForDateGreaterThanParamsToDate = agent.Accounts.CreditLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).ToList();
                var debitEntriesForDateGreaterThanParamsToDate = agent.Accounts.DebitLedgarEntries.Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).ToList();

                var agentBalanceForDateOf = debitValueForDateGreaterThanParamsToDate + (-creditValueForDateGreaterThanParamsToDate);

                decimal debitBalance = agentBalanceForDateOf;
                // decimal creditBalance = 0;
                var _value = (@params.Page - 1) * @params.ItemsPerPage;


                var list = entries.ToList();


                foreach (var item in list)
                {
                    count++;
                    var isItemSalesInvoice = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Invoice ||

                    x.Transaction.TransactionType == TransactionTypes.Payment
                        );
                    var isItemPayment = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Payment);
                    var isItemRefund = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Refund);
                    var isOpeningBalance = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.OpeningBalance);

                    AgentStatementDTO debit = new AgentStatementDTO();
                    if (isItemSalesInvoice)
                    {

                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Invoice;
                        if (item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id) != null)
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                            debitBalance += _actualItem.Amount;
                            debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                            debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault().PolicyNumber;
               
                            debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Make + " " + " | " +
                                        _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Model;
                            debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                            debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                            debit.Debit = item?.Value?.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Amount;
                            debit.Credit = 0;
                            debit.Balance = debitBalance;

                        }
                        else
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            debitBalance -= (item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount) ?? 0;

                            debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                            debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault()?.PolicyNumber;
                   
                            debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Make + " " + " | " +
                                        _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Model;
                            debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                            debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = debitBalance;

                        }
                    }
                    else if (isItemPayment)
                    {
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Payment;

                        var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                        debitBalance += _actualItem.Amount;
                        debit.Memo = _actualItem.Transaction?.Payment?.Memo;
                        debit.InvoiceDate = _actualItem?.Transaction?.Payment?.PaymentDate;
                        debit.RefNo = _actualItem?.Transaction?.Payment?.TransactionReferenceNumber;
                        debit.Debit = _actualItem?.Amount;
                        debit.Credit = (_actualItem?.Amount);
                        debit.Debit = 0;
                        debit.Balance = debitBalance;

                    }
                    else if (isOpeningBalance)
                    {
                        var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.OpeningBalance;
                        debit.Memo = "OPENING BALANCE";
                        debit.HasOnlyMemo = true;
                        debit.InvoiceDate = _actualItem.TransactionDate;
                        debitBalance += (_actualItem?.Amount) ?? 0;
                        debit.Debit = debitBalance;
                        debit.Balance = debitBalance;
                    }
                    else
                    {
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Refund;

                        var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                        debitBalance -= _actualItem.Amount;
                        debit.InvoiceDate = _actualItem?.Transaction?.Refund?.RefundDate;
                        debit.CustomerName = _actualItem?.Transaction?.Refund?.CustomerName;
                        debit.PolicyNumber = _actualItem?.Transaction?.Refund?.PolicyNumber;

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





                    if (list.IndexOf(item) == 0)
                    {
                        var entry = new AgentStatementDTO();
                        entry.TransactionType = TransactionTypes.OpeningBalance;
                        entry.Memo = "Opening balance as balance for date before " + @params.from.ToDateTime().Date.ToShortDateString();
                        entry.Balance = agentBalanceForDateOf;
                        entry.Num = count;
                        count++;
                    }
                    accountStatement.Add(debit);

                }

                OtherConstants.isSuccessful = true;
                if (@params.RequestPdf != null)
                {
                    var serverUrl = this.HttpContext.Request.Host.ToString();
                    var statemenmtPDF = new AccountStatementPDF();
                    statemenmtPDF.AccountTRN = agent.Id.ToString() + "/" + agent.CreateTime?.ToBinary().ToString() + "/" + DateTime.Now.Ticks;
                    statemenmtPDF.AgentName = agent.DisplayNameAs;
                    statemenmtPDF.DateFrom = @params.from.ToDateTime().ToShortDateString();
                    statemenmtPDF.DateTo = @params.to.ToDateTime().ToShortDateString();
                    statemenmtPDF.Country = "United Arab Emirates";
                    statemenmtPDF.Emirates = "Dubai";
                    statemenmtPDF.Statement = accountStatement;
                    var render = await _engine.RenderViewToStringAsync("GetAgentPaginated", statemenmtPDF);
                    var doc = new HtmlToPdfDocument()
                    {
                        GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Landscape,
                        PaperSize = PaperKind.A4Plus,
                    },
                        Objects = {
                        new ObjectSettings() {

                            PagesCount = true,
                            HtmlContent = render,
                            LoadSettings =
                            {
                                JSDelay=1000,
                                StopSlowScript=false,

                            },
                            WebSettings = { DefaultEncoding = "utf-8",
                            LoadImages=true,
                            EnableIntelligentShrinking=true,
                            EnableJavascript=true,
                            enablePlugins=true,
                            PrintMediaType=true
                            ,Background=true,

                            },
                            HeaderSettings =
                            {
                                HtmUrl=$"http://localhost:{Port}/header?id={agent.Id}&from={ statemenmtPDF.DateFrom }&to={statemenmtPDF.DateTo}&balance={debitBalance}"

                            }

                        }
                    }
                    };
                    byte[] pdf = _converter.Convert(doc);
                    string wwwPath = _env.WebRootPath;
                    string contentPath = _env.ContentRootPath;
                    var ransomeNameStr = new Random().Next(DateTime.Now.Second, 10000).ToString() +
                    new DateTime().Ticks.ToString();

                    var isHttps = this.HttpContext.Request.IsHttps;
                    var serverPath = isHttps ? "https://" : "http://" + serverUrl + $"/uploads/{ransomeNameStr}.pdf";
                    System.IO.File.WriteAllBytes(contentPath + $"\\Uploads\\{ransomeNameStr}.pdf", pdf);

                    page.PdfFileUrl = serverPath;
                }
                page.Data.AddRange(accountStatement.Skip((@params.Page - 1) * @params.ItemsPerPage).Take(@params.ItemsPerPage).ToList());
                return constructResponse(page);

            }

            //Wo case jab no date only search query for custromer or policy
            //0 0 1
            else if (@params.from == null && @params.to == null && @params.SearchQuery != null)
            {

                string searchString = @params.SearchQuery;
                #region Query
                var ledgers = query.Where(
         x =>

         (x.CreditAccountId == agent.DefaultAccountId || x.DebitAccountId == agent.DefaultAccountId)

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





        .ToList();
                #endregion
                #region Excel Export
                if (@params.RequestExcel != null)
                {

                    var groupedList = ledgers.OrderBy(x => x.TransactionDate).GroupBy(x => x.TransactionId).Select(

               x => new
               {
                   Key = x.Key,
                   Value = x
               }
               ).ToList();



                    decimal Balance = (agent.Accounts.DebitLedgarEntries
                        .Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date)
                                    .Sum(x => x.Amount)) + (-(agent.Accounts.CreditLedgarEntries.
                                    Where(x => x.TransactionDate.Date > @params.from.ToDateTime().Date).
                                    Sum(x => x.Amount)));

                    foreach (var item in groupedList)
                    {
                        count++;
                        var isItemSalesInvoice = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Invoice ||

                        x.Transaction.TransactionType == TransactionTypes.Payment
                            );
                        var isItemPayment = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Payment);
                        var isItemRefund = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Refund);
                        var isOpeningBalance = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.OpeningBalance);

                        AgentStatementDTO debit = new AgentStatementDTO();
                        if (isItemSalesInvoice)
                        {

                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.Invoice;
                            if (item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id) != null)
                            {
                                var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                                Balance += _actualItem.Amount;
                                debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                                debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                                debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault().PolicyNumber;
                    
                                debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                                debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Make + " " + " | " +
                                            _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Model;
                                debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                                debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                                debit.Debit = item?.Value?.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Amount;
                                debit.Credit = 0;
                                debit.Balance = Balance;

                            }
                            else
                            {
                                var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                                Balance -= (item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount) ?? 0;

                                debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                                debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                                debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault()?.PolicyNumber;
            
                                debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                                debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Make + " " + " | " +
                                            _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                            SingleOrDefault().Vehicle?.Model;
                                debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                                debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                                debit.Debit = _actualItem?.Amount;
                                debit.Credit = (_actualItem?.Amount);
                                debit.Debit = 0;
                                debit.Balance = Balance;

                            }
                        }
                        else if (isItemPayment)
                        {
                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.Payment;

                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            Balance += _actualItem.Amount;
                            debit.Memo = _actualItem.Transaction?.Payment?.Memo;
                            debit.InvoiceDate = _actualItem?.Transaction?.Payment?.PaymentDate;
                            debit.RefNo = _actualItem?.Transaction?.Payment?.TransactionReferenceNumber;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = Balance;

                        }
                        else if (isOpeningBalance)
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.OpeningBalance;
                            debit.Memo = "OPENING BALANCE";
                            debit.HasOnlyMemo = true;
                            debit.InvoiceDate = _actualItem.TransactionDate;
                            Balance += (_actualItem?.Amount) ?? 0;
                            debit.Debit = Balance;
                            debit.Balance = Balance;
                        }
                        else
                        {
                            debit.Num = count;
                            debit.TransactionType = TransactionTypes.Refund;

                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            Balance -= _actualItem.Amount;
                            debit.InvoiceDate = _actualItem?.Transaction?.Refund?.RefundDate;
                            debit.CustomerName = _actualItem?.Transaction?.Refund?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.Refund?.PolicyNumber;

                            debit.InsuranceType = _actualItem?.Transaction?.Refund?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.Refund?.Vehicle?.Make + " " + " | " + // Make 
                                                                                                            //Concating strings
                                        _actualItem?.Transaction?.Refund?.Vehicle?.Model; // Model

                            debit.Memo = _actualItem?.Transaction?.Refund?.MessageOnStatement; ;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = Balance;

                        }






                        accountStatement.Add(debit);

                    }

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
                #endregion

                #region Operation
                var entries = ledgers.OrderBy(x => x.TransactionDate).GroupBy(x => x.TransactionId).Select(

                    x => new
                    {
                        Key = x.Key,
                        Value = x
                    }
                    ).ToList();



                count = ((@params.ItemsPerPage > 10) || (@params.Page > 1)) ? @params.ItemsPerPage : 0;
                page.TotalPages = entries.Count() / @params.ItemsPerPage + (entries.Count() % @params.ItemsPerPage > 0 ? 1 : 0);
                decimal debitBalance = (query.Where(

                    x => x.DebitAccountId == agent.DefaultAccountId
                    && x.TransactionDate.Date < @params.from.ToDateTime().Date
                    ).Sum(x => x.Amount)) - (query.Where(

                    x => x.CreditAccountId == agent.DefaultAccountId
                    && x.TransactionDate.Date < @params.from.ToDateTime().Date
                    ).Sum(x => x.Amount));
                page.TotalCount = entries.Count();
                // decimal creditBalance = 0;
                var _value = (@params.Page - 1) * @params.ItemsPerPage;


                var list = entries.ToList();
                //var lastItem = list.Last();

                foreach (var item in list)
                {
                    count++;
                    var isItemSalesInvoice = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Invoice ||

                    x.Transaction.TransactionType == TransactionTypes.Payment
                        );
                    var isItemPayment = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Payment);
                    var isItemRefund = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.Refund);
                    var isOpeningBalance = item.Value.All(x => x.Transaction.TransactionType == TransactionTypes.OpeningBalance);

                    AgentStatementDTO debit = new AgentStatementDTO();
                    if (isItemSalesInvoice)
                    {

                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Invoice;
                        if (item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id) != null)
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                            debitBalance += _actualItem.Amount;
                            debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                            debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault().PolicyNumber;
                   
                            debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Make + " " + " | " +
                                        _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Model;
                            debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                            debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                            debit.Debit = item?.Value?.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Amount;
                            debit.Credit = 0;
                            debit.Balance = debitBalance;

                        }
                        else
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            debitBalance -= (item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount) ?? 0;

                            debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                            debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault()?.PolicyNumber;
                     
                            debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Make + " " + " | " +
                                        _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Model;
                            debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                            debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = debitBalance;

                        }
                    }
                    else if (isItemPayment)
                    {
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Payment;

                        var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                        debitBalance += _actualItem.Amount;
                        debit.Memo = _actualItem.Transaction?.Payment?.Memo;
                        debit.InvoiceDate = _actualItem?.Transaction?.Payment?.PaymentDate;
                        debit.RefNo = _actualItem?.Transaction?.Payment?.TransactionReferenceNumber;
                        debit.Debit = _actualItem?.Amount;
                        debit.Credit = (_actualItem?.Amount);
                        debit.Debit = 0;
                        debit.Balance = debitBalance;

                    }
                    else if (isOpeningBalance)
                    {
                        var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.OpeningBalance;
                        debit.Memo = "OPENING BALANCE";
                        debit.HasOnlyMemo = true;
                        debit.InvoiceDate = _actualItem.TransactionDate;
                        debitBalance += (_actualItem?.Amount) ?? 0;
                        debit.Debit = debitBalance;
                        debit.Balance = debitBalance;
                    }
                    else
                    {
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Refund;

                        var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                        debitBalance -= _actualItem.Amount;
                        debit.InvoiceDate = _actualItem?.Transaction?.Refund?.RefundDate;
                        debit.CustomerName = _actualItem?.Transaction?.Refund?.CustomerName;
                        debit.PolicyNumber = _actualItem?.Transaction?.Refund?.PolicyNumber;

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






                    accountStatement.Add(debit);

                }
                #endregion

                if (@params.RequestPdf!=null)
                {
                    var serverUrl = this.HttpContext.Request.Host.ToString();
                  
                    var statemenmtPDF = new AccountStatementPDF();
                    statemenmtPDF.AccountTRN = agent.Id.ToString() + "/" + agent.CreateTime?.ToBinary().ToString() + "/" + DateTime.Now.Ticks;
                    statemenmtPDF.AgentName = agent.DisplayNameAs;
                    statemenmtPDF.DateFrom = @params.from.ToDateTime().ToShortDateString();
                    statemenmtPDF.DateTo = @params.to.ToDateTime().ToShortDateString();
                    statemenmtPDF.Country = "United Arab Emirates";
                    statemenmtPDF.Emirates = "Dubai";
                    statemenmtPDF.Statement = accountStatement;
                    var render = await _engine.RenderViewToStringAsync("GetAgentPaginated", statemenmtPDF);
                    var doc = new HtmlToPdfDocument()
                    {
                        GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Landscape,
                        PaperSize = PaperKind.A4Plus,
                    },
                        Objects = {
                        new ObjectSettings() {

                            PagesCount = true,
                            HtmlContent = render,
                            LoadSettings =
                            {
                                JSDelay=1000,
                                StopSlowScript=false,

                            },
                            WebSettings = { DefaultEncoding = "utf-8",
                            LoadImages=true,
                            EnableIntelligentShrinking=true,
                            EnableJavascript=true,
                            enablePlugins=true,
                            PrintMediaType=true
                            ,Background=true,

                            },
                            HeaderSettings =
                            {
                                HtmUrl=$"http://{serverUrl}:{Port}/header?id={agent.Id}&from={ statemenmtPDF.DateFrom }&to={statemenmtPDF.DateTo}&balance={debitBalance}"

                            }

                        }
                    }
                    };
                    byte[] pdf = _converter.Convert(doc);
                    string wwwPath = _env.WebRootPath;
                    string contentPath = _env.ContentRootPath;
                    var ransomeNameStr = new Random().Next(DateTime.Now.Second, 10000).ToString() +
                    new DateTime().Ticks.ToString();
             
                    var isHttps = this.HttpContext.Request.IsHttps;
                    var serverPath = isHttps ? "https://" : "http://" + serverUrl + $"/uploads/{ransomeNameStr}.pdf";
                    System.IO.File.WriteAllBytes(contentPath + $"\\Uploads\\{ransomeNameStr}.pdf", pdf);

                    page.PdfFileUrl = serverPath;
                }

                OtherConstants.isSuccessful = true;
                page.Data.AddRange(accountStatement.Skip((@params.Page - 1) * @params.ItemsPerPage).Take(@params.ItemsPerPage).ToList());
                return constructResponse(page);

            }

            //all
            else
            {

                #region Query
                var ledgers = query.Where(
     x =>

     (x.CreditAccountId == agent.DefaultAccountId || x.DebitAccountId == agent.DefaultAccountId)




   )
         .OrderBy(x => x.TransactionDate)
                    .ToList();
                #endregion

                var entries = ledgers.GroupBy(x => x.TransactionId).Select(

                    x => new
                    {
                        Key = x.Key,
                        Value = x
                    }
                    ).ToList();


                count = ((@params.ItemsPerPage > 10) || (@params.Page > 1)) ? @params.ItemsPerPage : 0;
                page.TotalPages = entries.Count() / @params.ItemsPerPage + (entries.Count() % @params.ItemsPerPage > 0 ? 1 : 0);
                var prevDebitBalance = _context.Set<LedgarEntries>().AsNoTracking().Where(x =>


               (x.DebitAccountId == agent.DefaultAccountId)
                &&
              (x.TransactionDate.Date < @params.from.ToDateTime().Date)).Sum(x => x.Amount);

                decimal debitBalance = 0;
                page.TotalCount = entries.Count();
                // decimal creditBalance = 0;


                var list = entries.ToList();
                //var lastItem = list.Last();`

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

                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Invoice;
                        if (item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id) != null)
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                            debitBalance += _actualItem.Amount;
                            debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                            debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault().PolicyNumber;
                   
                            debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Make + " " + " | " +
                                        _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Model;
                            debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                            debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                            debit.Debit = item?.Value?.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Amount;
                            debit.Credit = 0;
                            debit.Balance = debitBalance;

                        }
                        else
                        {
                            var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                            debitBalance -= (item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount) ?? 0;

                            debit.InvoiceDate = _actualItem?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                            debit.CustomerName = _actualItem?.Transaction?.SalesInvoice?.CustomerName;
                            debit.PolicyNumber = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault()?.PolicyNumber;
                 
                            debit.InsuranceType = _actualItem?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                            debit.Vehicle = _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Make + " " + " | " +
                                        _actualItem?.Transaction?.SalesInvoice?.SaleLineItem?.
                                        SingleOrDefault().Vehicle?.Model;
                            debit.BodyType = _actualItem?.Transaction?.SalesInvoice?.BodyType?.Name;
                            debit.RefNo = _actualItem?.Transaction?.TransactionReferenceNumber;
                            debit.Debit = _actualItem?.Amount;
                            debit.Credit = (_actualItem?.Amount);
                            debit.Debit = 0;
                            debit.Balance = debitBalance;

                        }
                    }
                    else if (isItemPayment)
                    {
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Payment;

                        var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                        debitBalance += _actualItem.Amount;
                        debit.Memo = _actualItem.Transaction?.Payment?.Memo;
                        debit.InvoiceDate = _actualItem?.Transaction?.Payment?.PaymentDate;
                        debit.RefNo = _actualItem?.Transaction?.Payment?.TransactionReferenceNumber;
                        debit.Debit = _actualItem?.Amount;
                        debit.Credit = (_actualItem?.Amount);
                        debit.Debit = 0;
                        debit.Balance = debitBalance;

                    }
                    else if (isOpeningBalance)
                    {
                        var _actualItem = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id);
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.OpeningBalance;
                        debit.Memo = "OPENING BALANCE";
                        debit.HasOnlyMemo = true;
                        debit.InvoiceDate = _actualItem.TransactionDate;
                        debitBalance += (_actualItem?.Amount) ?? 0;
                        debit.Debit = debitBalance;
                        debit.Balance = debitBalance;
                    }
                    else
                    {
                        debit.Num = count;
                        debit.TransactionType = TransactionTypes.Refund;

                        var _actualItem = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id);
                        debitBalance -= _actualItem.Amount;
                        debit.InvoiceDate = _actualItem?.Transaction?.Refund?.RefundDate;
                        debit.CustomerName = _actualItem?.Transaction?.Refund?.CustomerName;
                        debit.PolicyNumber = _actualItem?.Transaction?.Refund?.PolicyNumber;

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


                    accountStatement.Add(debit);

                }
                var paginatedResult = accountStatement.Skip((@params.Page - 1) * @params.ItemsPerPage).Take(@params.ItemsPerPage).ToList();
                // list = entries.Skip((@params.Page - 1) * @params.ItemsPerPage).Take(@params.ItemsPerPage).ToList();
                OtherConstants.isSuccessful = true;
          
                page.Data.AddRange(paginatedResult);
                page.TotalBalance = paginatedResult.Last().Balance;
                return constructResponse(page);


            }

        }
        [HttpGet("GetAgentPaginated")]
        public ActionResult GetAgentPaginated()
        {
            return new ViewResult();
        }

        [AllowAnonymous]
        [HttpGet("GetAgentWithBalancePaginatedAsync")]
        public async Task<BaseResponse> GetAgentWithBalancePaginatedAsync([FromQuery] PaginationParams<int> @params) =>
                         constructResponse(await _service.GetPaginatedAgentsWithBalance(@params));



        [HttpGet("GetCurrentAccountStatement")]
        public async Task<BaseResponse> GetCurrentAccountStatement(int agentId)
        {
            var agent = (await _service.Get(x => x.Id == agentId)).SingleOrDefault();
            var Debit_Balance = (await _entriesService.Get(x => x.CreditAccountId == agent.DefaultAccountId)).Sum(x => x.Amount);
            var Credit_Balance = (await _entriesService.Get(x => x.CreditAccountId == agent.DefaultAccountId)).Sum(x => x.Amount);
            var OpenBalance = Debit_Balance - Credit_Balance;
            var AgentOpenBalance = new CurrentAgentStatementDTO()
            {
                OpenBalance = OpenBalance
            };
            return constructResponse(AgentOpenBalance);
        }

    }
    public class CurrentAgentStatementDTO
    {
        public decimal OpenBalance { get; set; }

    }
    public class AccountStatementPDF
    {
        public string AccountTRN { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string AgentName { get; set; }
        public string PhoneNumber { get; set; }
        public string Emirates { get; set; }
        public string Country { get; set; }
        public string Balance { get; set; }
        public List<AgentStatementDTO> Statement { get; set; }
    }
}
