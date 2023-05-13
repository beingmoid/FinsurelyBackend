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


    public class AccountDetailTypeController : BaseController<AccountDetailType,int>
    {
        private readonly IAccountDetailTypeService _service;

        public AccountDetailTypeController(RequestScope requestScope,IAccountDetailTypeService service)
            :base(requestScope,service)
        {
            _service = service;
        }
        [HttpGet("FilterList")]
        public async Task<BaseResponse> FilterList(int accountTypeId) => constructResponse(await _service.GetDetails(accountTypeId));
    }
}
