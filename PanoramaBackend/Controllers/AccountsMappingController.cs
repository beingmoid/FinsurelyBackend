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
 
      
    }
}
