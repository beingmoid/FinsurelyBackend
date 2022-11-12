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

    public class ServiceController : BaseController<Service,int>
    {
        private readonly IServiceService _service;

        public ServiceController(RequestScope requestScope, IServiceService
            service)
            :base(requestScope,service)
        {
            _service = service;
        }

        [HttpGet("ServiceByPolicy/{id}")]
        public async Task<BaseResponse> ServiceByPolicy(int id)
        {
            var services = await _service.Get(x => x.PolicyTypeId == id);
            OtherConstants.isSuccessful = true;
            return constructResponse(services);
        }
     
    }
}
