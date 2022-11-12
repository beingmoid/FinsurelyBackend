using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    public class AccountsMappingController : BaseController<AccountsMapping,int>
    {
        private readonly IAccountsMappingService _service;

        public AccountsMappingController(RequestScope requestScope,IAccountsMappingService service)
            :base(requestScope,service)
        {
            _service = service;
        }
      
        //public async Task<BaseResponse> AccountsReceivableList()
        //{
        //    var result = (await _service.Get(x => x.AccountDetailTypeId == 1));
        //    OtherConstants.isSuccessful = true;
        //    OtherConstants.messageType = MessageType.Success;
        //    return constructResponse(result);
        //}
        //public async Task<BaseResponse> AccountsPayableList()
        //{
        //    var result = (await _service.Get(x => x.AccountDetailTypeId == 41));
        //    OtherConstants.isSuccessful = true;
        //    OtherConstants.messageType = MessageType.Success;
        //    return constructResponse(result);
        //}

        //public async Task<BaseResponse> ExpenseAccountsList()
        //{
        //    var result = (await _service.Get(x => x.AccountDetailTypeId == 41));
        //    OtherConstants.isSuccessful = true;
        //    OtherConstants.messageType = MessageType.Success;
        //    return constructResponse(result);
        //}
    }
}
