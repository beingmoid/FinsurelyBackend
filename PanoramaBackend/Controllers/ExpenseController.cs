using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static NukesLab.Core.Common.Constants;
using PanoramBackend.Data;
using Microsoft.AspNetCore.Authorization;
using PanoramaBackend.Services.Reports;
using FluentExcel;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace PanoramaBackend.Api.Controllers
{

    public class ExpenseController : BaseController<Expense,int>
    {
        private readonly AMFContext _context;
        private readonly IExpenseService _service;
        private readonly IWebHostEnvironment _env;

        public ExpenseController(RequestScope requestScope,IExpenseService
            service,
            IWebHostEnvironment env
            
            , AMFContext context)
            :base(requestScope,service)
        {
            _context = context;
            _service = service;
            _env = env;
        }
        public async override Task<BaseResponse> Get()
        {
            var data = await _service.Get(x => x.Include(x => x.ExpenseCategory).Include(x=>x.Account)
            .Include(x=>x.Branch)
            );
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = "Data fetched successfully";
            return constructResponse(data);
        }


        [HttpGet("GetPaginated")]
        public async Task<BaseResponse> GetPaginated([FromQuery] PaginationParams<int> @param)

        { 
           var data =  _context.Set<Expense>().Include(x => x.ExpenseCategory).Include(x=>x.Account).Include(x=>x.Branch);
            int count = data.Count();
            var total = count / param.ItemsPerPage + (count % param.ItemsPerPage > 0 ? 1 : 0);

            var skipValue = (param.Page - 1) * param.ItemsPerPage;
            var skips = await data.Skip(skipValue).Take(param.ItemsPerPage).ToListAsync();

                   OtherConstants.isSuccessful = true;
            OtherConstants.messageType = "Data fetched successfully";

            return constructResponse(new { data=skips,totalPages= total,totalCount=data.Count() });
        }
        [AllowAnonymous]
        [HttpPost("GetPaginatedWithSearch")]

        public async Task<BaseResponse> GetPaginatedWithSearch([FromBody] PaginationParams<int> @param)
        {
            var query = _context.Set<Expense>().AsNoTracking();
            List<Expense> data = new List<Expense>();
            // when all conditions are true which means search filters contain branch, search queryy and from and to dates
            if (param.BranchId !=null && param.SearchQuery!=null && (param.from!=null || param.to !=null) )
            {

                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                    .Where(

                    x =>  (x.BranchId == param.BranchId)
                    &&
                    ( x.ExpenseName.Contains(param.SearchQuery ?? ""))

                    &&

                    (x.ExpenseDate.Date >= param.from.ToDateTime().Date)

                    &&

                    (x.ExpenseDate.Date <= param.to.ToDateTime().Date)

                    ).ToListAsync());


            }
            //when condition is branch and searchQuery
            else if(param.BranchId != null && param.SearchQuery != null && (param.from == null || param.to ==  null))
            {
                          data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                     .Where(

                 x =>
                  (x.ExpenseName.Contains(param.SearchQuery ?? ""))
                  &&

                     (x.BranchId == param.BranchId)

                 ).ToListAsync());

            }
            // when condition is branch and Date
            else if (param.BranchId != null && (param.from != null && param.to != null))
            {

                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                   .Where(

                   x => 
                   (
                 (x.ExpenseDate.Date >= param.from.ToDateTime().Date)

                 &&

                 (x.ExpenseDate.Date <= param.to.ToDateTime().Date)
                   ) && (x.BranchId == param.BranchId)

                   ).ToListAsync());

            }
            // when there are only seachQuery and date
            else if (param.BranchId == null && param.SearchQuery == null && ((param.from != null && param.to != null)))
            {
                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                 .Where(

                 x => 
                 (x.ExpenseName.Contains(param.SearchQuery ?? ""))

                 &&

                 (x.ExpenseDate.Date >= param.from.ToDateTime().Date)

                 &&

                 (x.ExpenseDate.Date <= param.to.ToDateTime().Date)

                 ).ToListAsync());
            }
            //when only branch
            else if (param.BranchId != null && param.SearchQuery == null && ((param.from == null && param.to == null)))
            {
                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                 .Where(

                 x => (x.BranchId == param.BranchId)
     

                 ).ToListAsync());
            }
            // when only searchQuery
            else if(param.SearchQuery!=null && param.BranchId == null && ((param.from == null && param.to == null)))
            {
                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                 .Where(

                 x => 
                 (x.ExpenseName.Contains(param.SearchQuery ?? ""))


                 ).ToListAsync());
            }
            // when only dates
            else
            {
                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                    .Where(

                    x => 

                    (x.ExpenseDate.Date >= param.from.ToDateTime().Date)

                    &&

                    (x.ExpenseDate.Date <= param.to.ToDateTime().Date)

                    ).ToListAsync());
            }

            int count = data.Count();
            var total = count / param.ItemsPerPage + (count % param.ItemsPerPage > 0? 1 : 0);
                var skips = data.Skip((param.Page - 1) * param.ItemsPerPage).OrderBy(x=>x.ExpenseDate).Take(param.ItemsPerPage).ToList();
                OtherConstants.isSuccessful = true;
                OtherConstants.messageType = "Data fetched successfully";
            return constructResponse(new { data=skips,totalPages= total,totalCount= count});

        }

        [AllowAnonymous]
        [HttpPost("GetExcel")]
        public async Task<BaseResponse> GetExcelFile([FromBody] PaginationParams<int> @param)
        {

            var query = _context.Set<Expense>().AsNoTracking();
            List<Expense> data = new List<Expense>();
            // when all conditions are true which means search filters contain branch, search queryy and from and to dates
            if (param.BranchId != null && param.SearchQuery != null && (param.from != null || param.to != null))
            {

                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                    .Where(

                    x => (x.BranchId == param.BranchId)
                    &&
                    (x.ExpenseName.Contains(param.SearchQuery ?? ""))

                    &&

                    (x.ExpenseDate.Date >= param.from.ToDateTime().Date)

                    &&

                    (x.ExpenseDate.Date <= param.to.ToDateTime().Date)

                    ).ToListAsync());


            }
            //when condition is branch and searchQuery
            else if (param.BranchId != null && param.SearchQuery != null && (param.from == null || param.to == null))
            {
                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
           .Where(

       x =>
        (x.ExpenseName.Contains(param.SearchQuery ?? ""))
        &&

           (x.BranchId == param.BranchId)

       ).ToListAsync());

            }
            // when condition is branch and Date
            else if (param.BranchId != null && (param.from != null && param.to != null))
            {

                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
              .Where(

              x =>
              (
            (x.ExpenseDate.Date >= param.from.ToDateTime().Date)

            &&

            (x.ExpenseDate.Date <= param.to.ToDateTime().Date)
              ) && (x.BranchId == param.BranchId)

              ).ToListAsync());

            }
            // when there are only seachQuery and date
            else if (param.BranchId == null && param.SearchQuery == null && ((param.from != null && param.to != null)))
            {
                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                 .Where(

                 x =>
                 (x.ExpenseName.Contains(param.SearchQuery ?? ""))

                 &&

                 (x.ExpenseDate.Date >= param.from.ToDateTime().Date)

                 &&

                 (x.ExpenseDate.Date <= param.to.ToDateTime().Date)

                 ).ToListAsync());
            }
            //when only branch
            else if (param.BranchId != null && param.SearchQuery == null && ((param.from == null && param.to == null)))
            {
                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                 .Where(

                 x => (x.BranchId == param.BranchId)


                 ).ToListAsync());
            }
            // when only searchQuery
            else if (param.SearchQuery != null && param.BranchId == null && ((param.from == null && param.to == null)))
            {
                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                 .Where(

                 x =>
                 (x.ExpenseName.Contains(param.SearchQuery ?? ""))


                 ).ToListAsync());
            }
            // when only dates
            else
            {
                data = (await query.Include(x => x.Branch).Include(x => x.Account).Include(x => x.ExpenseCategory)
                    .Where(

                    x =>

                    (x.ExpenseDate.Date >= param.from.ToDateTime().Date)

                    &&

                    (x.ExpenseDate.Date <= param.to.ToDateTime().Date)

                    ).ToListAsync());
            }


            ExpenseExcel.ConfigureExcel();
            var excelDto = new List<ExpenseExcel>();
            foreach (var item in data)
            {
                ExpenseExcel expense = new ExpenseExcel();
                expense.ExpenseDate = item.ExpenseDate.ToShortDateString();
                expense.Branch = item.Branch.BranchName;
                expense.ExpenseName = item.ExpenseName;
                expense.ExpenseCategory = item.ExpenseCategory.Name;
                expense.ExpenseAccount = item.Account.Name;
                expense.ExpenseAmount = item.ExpenseAmount.ToString();
                excelDto.Add(expense);
            }
            string wwwPath = _env.WebRootPath;
            string contentPath = _env.ContentRootPath;
            string path = Path.Combine(contentPath, $"\\uploads\\{new DateTime().Ticks.ToString()}.xlsx");
            var serverUrl = this.HttpContext.Request.Host.ToString();
            var ransomeNameStr = new Random().Next(DateTime.Now.Second, 10000).ToString()+
                new DateTime().Ticks.ToString();
            var isHttps = this.HttpContext.Request.IsHttps;

            var serverPath = isHttps? "https://": "http://"+serverUrl+$"/uploads/{ransomeNameStr}.xlsx";
            excelDto.ToExcel(contentPath+ $"\\uploads\\{ransomeNameStr}.xlsx");

            OtherConstants.isSuccessful = true;
           
            var portUrl = this.HttpContext.Request.Path;
            return constructResponse(new { filePath = serverPath });

      

        } 
























    }
}
