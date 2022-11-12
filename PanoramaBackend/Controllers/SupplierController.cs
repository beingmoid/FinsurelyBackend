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
using Microsoft.EntityFrameworkCore;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Api.Controllers
{

    public class SupplierController : BaseController<UserDetails,int>
    {
        private readonly ISupplierService _service;

        public SupplierController(RequestScope requestScope,ISupplierService service)
            :base(requestScope,service)
        {
            _service = service;
        }
        public async override Task<BaseResponse> Get(int id)
        {
            var userDetail = (await _service.Get(x =>
              x.Include(x => x.Addresses)
             .Include(x => x.PaymentAndBilling)
               .ThenInclude(x => x.Terms)
             .Include(x => x.PaymentAndBilling)
              .ThenInclude(x => x.PreferredPaymentMethod)
              .Include(x => x.UserDetail)
              .Include(x => x.SalesInvoicePersons).ThenInclude(x => x.Transactions),
              x => x.Id == id
             )).SingleOrDefault();
            OtherConstants.isSuccessful = true;
            return constructResponse(userDetail);

        }
    }
}
