using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    public class SetupClientController : BaseController<SetupClient, int>
    {
        public SetupClientController(RequestScope requestScope, ISetupClientService service)
            : base(requestScope, service)
        {

        }
    }
}
