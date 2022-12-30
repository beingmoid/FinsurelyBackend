using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaBackend.Api.Controllers
{

    public class VacationApplicationController : BaseController<VacationApplication,int>
    {
        private readonly AMFContext _context;

        public VacationApplicationController(RequestScope requestScope,IVacationApplicationService
            service, AMFContext context)
            :base(requestScope,service)
        {
            _context = context;
        }

        //public override Task<BaseResponse> Get()
        //{
            
        //}
    }
}
