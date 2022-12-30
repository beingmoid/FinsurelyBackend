using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Api.Controllers
{
 
    public class AnnouncementController : BaseController<Announcement,int>
    {
        private readonly IAnnoucementService _service;

        public AnnouncementController(RequestScope requestScope,IAnnoucementService service)
            :base(requestScope,service)
        {
            _service=service;
        }
        public override async Task<BaseResponse> Get()
        {
            var data = (await _service.Get()).GroupBy(x=>x.Date).Select(x=> new 
            {
                Date =x.Key,
                Value=x

            });
            OtherConstants.isSuccessful= true;

            return constructResponse(data);
            
        }
    }
}
