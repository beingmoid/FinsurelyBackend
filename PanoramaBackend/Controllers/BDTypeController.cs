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

namespace PanoramaBackend.Api.Controllers
{

    public class BDTypeController : BaseController<BDType,int>
    {
        private readonly IBDTypeService _service;

        public BDTypeController(RequestScope requestScope,IBDTypeService
            service)
            :base(requestScope,service)
        {
            _service = service;
        }
        [HttpGet("Benefits")]
        public async Task<BaseResponse> Benefits()
        {
            var result = (await _service.Get(x => x.IsForBenefit==true)).ToList();
            return constructResponse(result);
        }
        [HttpGet("Deductions")]
        public async Task<BaseResponse> Deduction()
        {
            var result = (await _service.Get(x => x.IsForDeduction == true)).ToList();
            return constructResponse(result);
        }
    }
}
