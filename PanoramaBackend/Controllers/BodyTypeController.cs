using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
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

    public class BodyTypeController : BaseController<BodyType, int>
    {
        private readonly IBodyTypeService _service;

        public BodyTypeController(RequestScope requestScope, IBodyTypeService
            service)
            : base(requestScope, service)
        {
            _service = service;
        }

    }

}
