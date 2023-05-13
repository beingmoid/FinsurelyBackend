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

namespace PanoramaBackend.Api.Controllers
{

    public class ReconcilationController : BaseController<Reconcilation,int>
    {
        private readonly IReconcilationService _service;

        public ReconcilationController(RequestScope requestScope,IReconcilationService
            service)
            :base(requestScope,service)
        {
            _service = service;
        }

        [HttpPost("Process")]
        public async Task<BaseResponse> GetProcess([FromBody] Recon recon)
        {
            return constructResponse(await _service.ProcessReconcilation(recon));
        }
    }
  
}
