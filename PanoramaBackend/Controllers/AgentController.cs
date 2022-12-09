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

namespace PanoramaBackend.Api.Controllers
{

    public class AgentController : BaseController<UserDetails, int>
    {
        private readonly IAgentService _service;
        private readonly ILedgerEntriesService _entriesService;
        private readonly ITransactionService _tranService;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _env;

        public AgentController(RequestScope requestScope, IAgentService service, ILedgerEntriesService entriesService
            , ITransactionService tranService,
            IWebHostEnvironment env,
            IConverter converter            )
            : base(requestScope, service)
        {
            _service = service;
            _entriesService = entriesService;
            _tranService = tranService;
            _converter = converter;
            _env = env;
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
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.Service)
        .Include(x => x.Transaction)
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.PolicyType)
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
            //int count = 0;
            //page.TotalPages = ledgers.ToList().Count / pageNo;
            //page.CurrentPage = pageNo;
            //page.PageSize = pageSize;
            //decimal debitBalance = 0;
            return page;

        }

        [AllowAnonymous]
        [HttpGet("statement")]

        public async Task<BaseResponse> GetAccountStatement([FromQuery] PaginationParams<int> @params)
        {

            var agent = (await _service.Get(x => x.Include(x => x.Accounts), x => x.Id == @params.Id)).SingleOrDefault();

            if (agent == null) return constructResponse(NotFound());

            var ledgers = (await _entriesService.Get(x => x.Include(x => x.Transaction)

            .ThenInclude(x => x.SalesInvoice)
                        .ThenInclude(x => x.SaleLineItem)
                            .ThenInclude(x => x.Vehicle)

            .Include(x => x.Transaction)
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.InsuranceType)
        .Include(x => x.Transaction)
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.Branch)
            .Include(x => x.Transaction)
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.Service)
        .Include(x => x.Transaction)
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.PolicyType)
        .Include(x => x.Transaction)
                   .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.BodyType)

                   .Include(x=>x.Transaction)

            , x => x.CreditAccountId == agent.DefaultAccountId || x.DebitAccountId==agent.DefaultAccountId)).OrderBy(x=>x.Id).ToList();


            var entries = ledgers.GroupBy(x => x.TransactionId).Select(

                x => new
                {
                    Key = x.Key,
                    Value = x
                }
                ).ToList();
            List<dynamic> accountStatement = new List<dynamic>();
            PageConfig page = new PageConfig();
            int count = 0;
            page.TotalPages = entries.Count() / @params.ItemsPerPage + (entries.Count() % @params.ItemsPerPage > 0 ? 1 : 0);
            decimal debitBalance = 0;
            // decimal creditBalance = 0;
            var _value = (@params.Page - 1) * @params.ItemsPerPage;
            var _skipCount = (@params.Page - 1) * @params.ItemsPerPage;
            bool dontSkip = false;
            if (_skipCount > entries.Count)
            {
                dontSkip = true;

            }
            else 
                dontSkip = false;


            var list = dontSkip? entries.Take(@params.ItemsPerPage).ToList(): entries.Skip((@params.Page-1) * @params.ItemsPerPage).Take(@params.ItemsPerPage).ToList();
            //var lastItem = list.Last();
            foreach (var item in list)
            {
                count++;

                AgentStatementDTO debit = new AgentStatementDTO();
                debit.Num = count;
                if (item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id) != null)
                {

                    debitBalance += item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id).Amount;

                    
                    debit.InvoiceDate = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                    debit.CustomerName = "x => x.DebitAccountId ==agent.Accounts.Id";
                    debit.PolicyNumber = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault().PolicyNumber;
                    debit.PolicyType = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.PolicyType?.Name;
                    debit.ServiceType = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.Service?.Name;
                    debit.InsuranceType = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                    debit.Vehicle = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Make + " " + " | " +
                                item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Model;
                    debit.BodyType = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.BodyType?.Name;
                    debit.RefNo = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.TransactionReferenceNumber;
                    debit.Debit = item?.Value?.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Amount;
                    debit.Credit = 0;
                    debit.Balance = debitBalance;

                }
                else
                {
                    debitBalance += -(item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount) ?? 0;

                    debit.InvoiceDate = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SalesInvoiceDate;
                    debit.CustomerName = "x => x.CreditAccountId ==agent.Accounts.Id";
                    debit.PolicyNumber = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.SingleOrDefault()?.PolicyNumber;
                    debit.PolicyType = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.PolicyType?.Name;
                    debit.ServiceType = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.Service?.Name;
                    debit.InsuranceType = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                    debit.Vehicle = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Make + " " + " | " +
                                item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Model;
                    debit.BodyType = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.BodyType?.Name;
                    debit.RefNo = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.TransactionReferenceNumber;
                    debit.Debit = item?.Value?.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount;
                    debit.Credit = -(item?.Value?.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount);
                    debit.Debit = 0;
                    debit.Balance = debitBalance;
                }


                accountStatement.Add(debit);


            }


            OtherConstants.isSuccessful = true;
            page.Data.AddRange(accountStatement);
            return constructResponse(page);




        }


        [AllowAnonymous]
        [HttpGet("Download")]
        public async Task<ActionResult> DownloadStatement(int agentId, DateTime from , DateTime to)
        {
            string path = Path.Combine(_env.ContentRootPath, "EmailTemplates\\statement.html");
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
            .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.Service)
 .Include(x => x.Transaction)
            .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.PolicyType)
 .Include(x => x.Transaction)
            .ThenInclude(x => x.SalesInvoice).ThenInclude(x => x.BodyType)



     , x => (x.TransactionDate>=from && x.TransactionDate<=to ) && x.Transaction.UserDetailId == agentId)).GroupBy(x => x.TransactionId).Select(

         x => new
         {
             Key = x.Key,
             Value = x
         }
         );
            decimal debitBalance = 0;
            List<dynamic> accountStatement = new List<dynamic>();
            PageConfig page = new PageConfig();
            int count = 0;
            foreach (var item in ledgers)
            {
                count++;

                AgentStatementDTO debit = new AgentStatementDTO();
                debit.Num = count;
                if (item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id) != null)
                {

                    debitBalance += item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id).Amount;


                    debit.InvoiceDate = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id).Transaction.SalesInvoice.SalesInvoiceDate;
                    debit.CustomerName = "x => x.DebitAccountId ==agent.Accounts.Id";
                    debit.PolicyNumber = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id).Transaction.SalesInvoice.SaleLineItem.SingleOrDefault().PolicyNumber;
                    debit.PolicyType = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.PolicyType?.Name;
                    debit.ServiceType = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.Service?.Name;
                    debit.InsuranceType = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                    debit.Vehicle = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Make + " " + " | " +
                                item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Model;
                    debit.BodyType = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.BodyType?.Name;
                    debit.RefNo = item.Value.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Transaction?.TransactionReferenceNumber;
                    debit.Debit = item?.Value?.SingleOrDefault(x => x.DebitAccountId == agent.Accounts.Id)?.Amount;
                    debit.Credit = 0;
                    debit.Balance = debitBalance;

                }
                else
                {
                    debitBalance += -(item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id).Amount);

                    debit.InvoiceDate = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id).Transaction.SalesInvoice.SalesInvoiceDate;
                    debit.CustomerName = "x => x.CreditAccountId ==agent.Accounts.Id";
                    debit.PolicyNumber = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id).Transaction.SalesInvoice.SaleLineItem.SingleOrDefault().PolicyNumber;
                    debit.PolicyType = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.PolicyType?.Name;
                    debit.ServiceType = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.Service?.Name;
                    debit.InsuranceType = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.InsuranceType?.Name;
                    debit.Vehicle = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Make + " " + " | " +
                                item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.SaleLineItem?.
                                SingleOrDefault().Vehicle?.Model;
                    debit.BodyType = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.SalesInvoice?.BodyType?.Name;
                    debit.RefNo = item.Value.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Transaction?.TransactionReferenceNumber;
                    debit.Debit = item?.Value?.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount;
                    debit.Credit = -(item?.Value?.SingleOrDefault(x => x.CreditAccountId == agent.Accounts.Id)?.Amount);
                    debit.Debit = 0;
                    debit.Balance = debitBalance;
                }


                accountStatement.Add(debit);


            }


            //OtherConstants.isSuccessful = true;
            page.Data.AddRange(accountStatement);



   
            if (System.IO.File.Exists(path))
            {
                string html = System.IO.File.ReadAllText(path);
                DotLiquid.Template template = DotLiquid.Template.Parse(html);

                string renderedHtml = template.Render(Hash.FromAnonymousObject(new
                {
                    accountNo = agent.DisplayNameAs.First() + agent.DisplayNameAs.Last() + agent.DefaultAccountId + agentId.ToString(),
                    agentName = agent.DisplayNameAs,
                    phoneNumber = agent.Phone,
                    emirates = "AJMAN",
                    source=page.Data,
                    country = "UNITED ARAB EMIRATES",
                    dateFrom = from.ToString("MMMM d, yyyy"),
                    dateTo = to.ToString("MMMM d, yyyy")
                })); ;
                
           
                var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4Plus,
    },
                Objects = {
        new ObjectSettings() {
            PagesCount = true,
            HtmlContent = renderedHtml,
            WebSettings = { DefaultEncoding = "utf-8" },
            HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
        }
    }
            };
            byte[] pdf = _converter.Convert(doc);




            return new FileContentResult(pdf, "application/pdf");
            }
            return NotFound();

        }

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
}
