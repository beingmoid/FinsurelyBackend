using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Api.Controllers
{

    public class ComissionrateController : BaseController<ComissionRate, int>
    {
        private readonly IComissionRateService _service;

        public ComissionrateController(RequestScope requestScope,IComissionRateService
            service)
            :base(requestScope, service)
        {
            _service = service;
        }
        [HttpGet("GetRates")]
        public async Task<BaseResponse> GetRates(int userDetailId)
        {
            OtherConstants.isSuccessful = true;
            return constructResponse(await _service.Get(x => x.UserDetailId== userDetailId && x.IsActive));
        }
    }
}
