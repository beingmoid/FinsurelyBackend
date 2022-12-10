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

namespace PanoramaBackend.Api.Controllers
{

    public class PolicyTypeController : BaseController<PolicyType, int>
    {
        private readonly IPolicyTypeService _service;

        public PolicyTypeController(RequestScope requestScope, IPolicyTypeService
            service)
            : base(requestScope, service)
        {
            _service = service;
        }
        //public override Task<BaseResponse> Get()
        //{

        //    return constructResponse(_service.Get(x=>x.Include(x=>x.PolicyType)))
        //}

    }

}
