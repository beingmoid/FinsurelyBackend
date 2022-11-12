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

namespace PanoramaBackend.Api.Controllers
{

    public class EmploymentStatusController : BaseController<EmploymentStatus,int>
    {
        public EmploymentStatusController(RequestScope requestScope,IEmploymentStatusService
            service)
            :base(requestScope,service)
        {

        }
    }
}
