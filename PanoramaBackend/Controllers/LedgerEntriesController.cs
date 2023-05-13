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
using Microsoft.EntityFrameworkCore;

namespace PanoramaBackend.Api.Controllers
{

    public class LedgerEntriesController : BaseController<LedgarEntries,int>
    {
        private readonly ILedgerEntriesService _service;

        public LedgerEntriesController(RequestScope requestScope,ILedgerEntriesService service)
            :base(requestScope,service)
        {
            _service = service;
        }


        [HttpGet("GetReceviableStatementReport")]
        public async Task<BaseResponse> GetAccountReceviableStatement(int accountId)
        {
           
            return constructResponse(await _service.GetAccountReceviableStatement( accountId));

        }
        [HttpGet("SearchAndFilter")]
        public async Task<BaseResponse> SearchAndFilter([FromBody] SearchAndFilter filter,int accountId, string start, string end, string sortBy)
        {

            return constructResponse(await _service.SearchAndFilter(accountId, filter.start, filter.end, filter.sortBy));

        }

    }

    
}
