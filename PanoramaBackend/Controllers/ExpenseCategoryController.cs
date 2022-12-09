using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
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
