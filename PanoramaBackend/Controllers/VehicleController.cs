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

    public class VehicleController : BaseController<Vehicle,int>
    {
        public VehicleController(RequestScope requestScope,IVehicleService
            service)
            :base(requestScope,service)
        {

        }
    }
}
