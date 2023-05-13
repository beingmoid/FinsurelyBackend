using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static NukesLab.Core.Common.Constants;
using Microsoft.AspNetCore.Authorization;

namespace PanoramaBackend.Api.Controllers
{

    [AllowAnonymous]
    public class CustomerController : BaseController<UserDetails,int>
    {
        private readonly ICustomerService _service;

        public CustomerController(RequestScope requestScope,ICustomerService service)
            :base(requestScope,service)
        {
            _service = service;
        }
        public async override Task<BaseResponse> Get(int id)
        {
            var userDetail =(await _service.Get(x =>
             x.Include(x => x.Addresses)
            .Include(x => x.PaymentAndBilling)
              .ThenInclude(x => x.Terms)
            .Include(x => x.PaymentAndBilling)
             .ThenInclude(x => x.PreferredPaymentMethod)
         
             .Include(x=>x.CustomerSalesInvoice).ThenInclude(x=>x.Vehicle)
              .Include(x => x.CustomerSalesInvoice).ThenInclude(x => x.InsuranceType)
              .Include(x => x.CustomerSalesInvoice).ThenInclude(x => x.InsuranceCompany)
             .Include(x=>x.Attachments)
             ,x=>x.Id==id
             )).SingleOrDefault();
            OtherConstants.isSuccessful = true;

            return constructResponse(userDetail);

        }
    }
}
