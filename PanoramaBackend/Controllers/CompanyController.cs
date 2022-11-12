using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramBackend.Data.CatalogDb;
using PanoramBackend.Data.CatalogDb.Repos;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Api.Controllers
{
   
    //public class CompanyController : BaseController<Company, int>
    //{
    //    private readonly ICompanyRepo _repo;

    //    public CompanyController(RequestScope requestScope, ICompanyRepo repo
    //        )
    //        : base(requestScope, null)
    //    {
    //        _repo = repo;
    //    }
    //    [AllowAnonymous]
    //    [HttpGet("ValidEmail")]
    //    public async Task<BaseResponse> ValidEmail(string email)
    //    {
    //        var tenant = (await _repo.Get(x => x.Email == email));
    //        if (tenant!=null)
    //        {
    //            OtherConstants.isSuccessful = true;
    //            return constructResponse(new { ValidationResult = true });
    //        }
    //        else
    //        {
    //            OtherConstants.isSuccessful = false;
    //            return constructResponse(new { ValidationResult = false });
    //        }
    //    }

    //    //[AllowAnonymous]
    //    //[HttpGet]
    //    //public async Task<BaseResponse> SendVerificationEmail(string email)
    //    //{

    //    //    return constructResponse();
    //    //}

    //    //[HttpPost]
    //    //public async Task<BaseResponse> OnboardCompany([FromBody] OnboardingTenant company)
    //    //{

    //    //    return constructResponse();
    //    //}
    //}

}


