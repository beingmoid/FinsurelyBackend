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
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Api.Controllers
{

    public class CompensationController : BaseController<Compensation,int>
    {
        private readonly ICompensationService _service;

        public CompensationController(RequestScope requestScope,ICompensationService
            service)
            :base(requestScope,service)
        {
            _service = service;
        }

        [HttpGet("GetCompensation")]
        public async  Task<BaseResponse> GetCompensations(int empId)
        {
            var data = (await _service.Get(x => x.EmploymentDetailId == empId)).ToList().OrderByDescending(x => x.EffectiveDate);
            OtherConstants.isSuccessful = true;
            return constructResponse(data);

        }
    }
}
