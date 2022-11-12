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

namespace PanoramaBackend.Api.Controllers
{

    public class EmployeeController : BaseController<UserDetails,int>
    {
        private readonly IEmployeeService _service;

        public EmployeeController(RequestScope requestScope,IEmployeeService service)
            :base(requestScope,service)
        {
            _service = service;
        }
        public async override Task<BaseResponse> Get()
        {
            var result = (await _service.Get(x=> x.Include(x => x.Addresses)
             .Include(x => x.PaymentAndBilling)
               .ThenInclude(x => x.Terms)
             .Include(x => x.PaymentAndBilling)
              .ThenInclude(x => x.PreferredPaymentMethod)
                    .Include(x=>x.AssignedTask).ThenInclude(x=>x.Status)
                    .Include(x => x.AssignedTask).ThenInclude(x => x.Priority)
              .Include(x => x.UserDetail)
              .Include(x => x.EmploymentDetails)
              .ThenInclude(x => x.Manager),x=>x.IsEmployee==true)).ToList();
            OtherConstants.isSuccessful = true;
            return constructResponse(result);
        }

        [HttpGet("GetManagers")]
        public async Task<BaseResponse> GetManagers()
        {
            var result = (await _service.Get(x => x.Include(x => x.EmploymentDetails),x=>x.EmploymentDetails.Any(x=>x.ManagerId==null))).ToList();
            OtherConstants.isSuccessful = true;
            return constructResponse(result);
        }
        public async override Task<BaseResponse> Get(int id)
        {
            var userDetail = (await _service.Get(x =>
              x.Include(x => x.Addresses)
                .Include(x=>x.EmploymentDetails)
                .ThenInclude(x=>x.Compensations)
                    .Include(x => x.EmploymentDetails)
                .ThenInclude(x => x.VacationPolicies)
                         .Include(x => x.EmploymentDetails)
                .ThenInclude(x => x.BenefitsAndDeductions)
                                         .Include(x => x.EmploymentDetails)
                .ThenInclude(x => x.EmployeeFiles)
                                          .Include(x => x.EmploymentDetails)
                .ThenInclude(x => x.BankDetails)
                                                      .Include(x => x.EmploymentDetails)
                .ThenInclude(x => x.EmploymentStatus)
       , x => x.Id == id
             )).SingleOrDefault();
            OtherConstants.isSuccessful = true;
            return constructResponse(userDetail);

        }
    }
}
