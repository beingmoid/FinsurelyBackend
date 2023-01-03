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

    public class RefundController : BaseController<Refund,int>
    {
        private readonly IRefundService _service;

        public RefundController(RequestScope requestScope,IRefundService
            service)
            :base(requestScope,service)
        {
            _service = service;
        }
        public override async Task<BaseResponse> Get()
        {
            var response = (await _service.Get(x => x.Include(x => x.Vehicle)
            .Include(x => x.PolicyType)
            .Include(x => x.InsuranceType)
            .Include(x => x.InsuranceCompany)
            .Include(x => x.Agent)
    
            )).OrderBy(x=>x.RefundDate).ToList();
            OtherConstants.isSuccessful = true;

            return constructResponse(response);
            
;
        }
    }
}
