using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaBackend.Api.Controllers
{
    [ResponseCache]
    public class AnnouncementController : BaseController<Announcement,int>
    {
        public AnnouncementController(RequestScope requestScope,IAnnoucementService service)
            :base(requestScope,service)
        {

        }
    }
}
