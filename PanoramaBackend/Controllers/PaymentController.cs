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

    public class PaymentController : BaseController<Payment,int>
    {
        private readonly IPaymentService _service;

        public PaymentController(RequestScope requestScope,IPaymentService
            service)
            :base(requestScope,service)
        {
            _service = service;
        }
        [HttpGet("GetReceivePayments")]
        public async Task<BaseResponse> GetReceivePayments(DateTime from , DateTime to)
        {
          return constructResponse(await _service.GetRecevingPayment(from, to));
        }
        [HttpGet("GetSentPayment")]
        public async Task<BaseResponse> GetSentPayment(DateTime from, DateTime to)
        {
            return constructResponse(await _service.GetSentPayment(from, to));
        }

        [HttpPost("ReceviePayment")]
        public async Task<BaseResponse> PaymentReceive([FromBody] Payment payment)
        {
            return constructResponse(await _service.ReceivePayment(payment));
        }

        [HttpPut("UpdateReceviePayment/{Id}")]
        public async Task<BaseResponse> UpdatePaymentReceive(int Id,[FromBody] Payment payment)
        {
            return constructResponse(await _service.UpdateReceviedPayment(Id,payment));
        }
        [HttpDelete("DeleteReceviePayment/{Id}")]
        public async Task<BaseResponse> DeletePaymentReceive(int Id)
        {
            return constructResponse(await _service.DeleteReceviedPayment(Id));
        }

        [HttpPost("SendPayment")]
        public async Task<BaseResponse> SendPayment([FromBody] Payment payment)
        {
            return constructResponse(await _service.SendPayment(payment));
        }

        [HttpPut("UpdatePaymentSent/{Id}")]
        public async Task<BaseResponse> UpdatePaymentSent(int Id, [FromBody] Payment payment)
        {
            return constructResponse(await _service.UpdateSendPayment(Id, payment));
        }
        [HttpDelete("DeletePaymentSent/{Id}")]
        public async Task<BaseResponse> DeletePaymentSent(int Id)
        {
            return constructResponse(await _service.DeleteSendPayment(Id));
        }

    }
}
