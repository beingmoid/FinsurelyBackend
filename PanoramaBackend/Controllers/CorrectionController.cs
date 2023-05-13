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

    public class CorrectionController : BaseController<CompanyInformation,int>
    {
        public CorrectionController(RequestScope requestScope,ICorrectionService
            service)
            :base(requestScope,service)
        {

        }
    }
}
