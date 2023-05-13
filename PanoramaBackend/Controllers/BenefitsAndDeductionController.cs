using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
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

    public class BenefitsAndDeductionController : BaseController<BenefitsAndDeduction,int>
    {
        private readonly IBenefitsAndDeductionService _service;

        public BenefitsAndDeductionController(RequestScope requestScope,IBenefitsAndDeductionService
            service)
            :base(requestScope,service)
        {
            _service = service;
        }
        [HttpGet("EmployeeBenefitsAndDeductions")]
        public async Task<BaseResponse> BenefitsAndDeduction(int empId)
        {
            var result =await  _service.Get(x => x.Include(x => x.Deduction).ThenInclude(x => x.Type)

            .Include(x => x.Benefits).ThenInclude(x => x.Type), x => x.EmploymentDetailId == empId);
            OtherConstants.isSuccessful = true;

            return constructResponse(result);
        }
    }
}
