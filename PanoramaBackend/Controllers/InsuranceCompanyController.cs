using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static NukesLab.Core.Common.Constants;
using PanoramBackend.Data.Repository;

namespace PanoramaBackend.Api.Controllers
{

    public class InsuranceCompanyController : BaseController<UserDetails,int>
    {
        private readonly IInsuranceCompanyService _service;
        private readonly IComissionRateService _comissionRateService;
        private readonly ILedgerEntriesService _entriesService;

        public InsuranceCompanyController(RequestScope requestScope,IInsuranceCompanyService service,
            ILedgerEntriesService entriesService,
            IComissionRateService comissionRateService)
            :base(requestScope,service)
        {
            _service = service;
            _comissionRateService = comissionRateService;
            _entriesService = entriesService;
        }
        public async override Task<BaseResponse> Get()
        {
            return constructResponse(await _service.GetCompaniesWithBalance());
        }
        public async override Task<BaseResponse> Get(int id)
        {
            var userDetail = (await _service.Get(x =>
              x.Include(x => x.Addresses)
              .Include(x => x.ComissionRates)
             .Include(x => x.PaymentAndBilling)
               .ThenInclude(x => x.Terms)
             .Include(x => x.PaymentAndBilling)
              .ThenInclude(x => x.PreferredPaymentMethod)
              .Include(x => x.UserDetail)
              .Include(x => x.InsuranceCompanyInvoices).ThenInclude(x => x.Transactions),
              x => x.Id == id

             )).SingleOrDefault();
           // userDetail.ComissionRates.RemoveAll(x => !x.IsActive);
               
             

            OtherConstants.isSuccessful = true;
            return constructResponse(userDetail);

        }

        [HttpGet("GetBalance")]
        public async Task<BaseResponse> GetBalance([FromQuery] int id)
        {
            var balance = await _service.GetBalance(id);
            return constructResponse(new { balance = balance });
        }
        [HttpGet("GetComissionRates")]
        public async Task<BaseResponse> GetComissionRates(int id,bool isTpl)
        {
            if (isTpl)
            {
                var comission = (await _comissionRateService.Get(x => x.UserDetailId == id && x.IsTpl && x.IsActive)).SingleOrDefault();
                return constructResponse(comission);
            }
            else
            {
                var comission = (await _comissionRateService.Get(x => x.UserDetailId == id && x.IsNonTpl && x.IsActive)).SingleOrDefault();
                return constructResponse(comission);
            }
           
        }
        [HttpGet("GetCurrentAccountStatement")]
        public async Task<BaseResponse> GetCurrentAccountStatement(int companyId)
        {
            var agent = (await _service.Get(x => x.Id == companyId)).SingleOrDefault();
            var Debit_Balance = (await _entriesService.Get(x => x.DebitAccountId == agent.DefaultAccountId)).Sum(x => x.Amount);
            var Credit_Balance = (await _entriesService.Get(x => x.CreditAccountId == agent.DefaultAccountId)).Sum(x => x.Amount);

            if (Debit_Balance > Credit_Balance)
            {
                var OpenBalance = Debit_Balance - Credit_Balance;
                var AgentOpenBalance = new CurrentAgentStatementDTO()
                {
                    OpenBalance = OpenBalance
                };
                return constructResponse(AgentOpenBalance);
            }
            else
            {
                var OpenBalance = Credit_Balance- Debit_Balance  ;
                var AgentOpenBalance = new CurrentAgentStatementDTO()
                {
                    OpenBalance = OpenBalance
                };
                return constructResponse(AgentOpenBalance);
            }
           
           
        }
    }
}
