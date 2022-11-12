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
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Api.Controllers
{

    public class BankDetailsController : BaseController<BankDetails,int>
    {
        private readonly IBankDetailService _service;

        public BankDetailsController(RequestScope requestScope,IBankDetailService
            service)
            :base(requestScope,service)
        {
            _service = service;
        }

        [HttpGet("GetBankDetails")]
        public async Task<BaseResponse> GetBankDetails(int empId)
        {
            var result = (await _service.Get(x => x.EmploymentDetailId == empId)).ToList();
            OtherConstants.isSuccessful = true;
            return constructResponse(result);
        }
    }
}
