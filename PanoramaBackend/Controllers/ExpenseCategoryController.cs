using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaBackend.Api.Controllers
{

    public class ExpenseCategoryController : BaseController<ExpenseCategory,int>
    {
        public ExpenseCategoryController(RequestScope requestScope,IExpenseCategoryService
            service)
            :base(requestScope,service)
        {

        }
    }
}
