
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Api.Controllers
{

    public class PayrollController : BaseController<Payroll,int>
    {
        private readonly IPayrollService _service;

        public PayrollController(RequestScope requestScope,IPayrollService
            service)
            :base(requestScope,service)
        {
            _service=service;
        }

        public async override Task<BaseResponse> Get()
        {
            var data = (await _service.Get(x => x.Include(x => x.Branch)
              .Include(x => x.ExpenseAccount)
            )).ToList();
            
            OtherConstants.isSuccessful = true;
            return constructResponse(data);
        }
    }
}
