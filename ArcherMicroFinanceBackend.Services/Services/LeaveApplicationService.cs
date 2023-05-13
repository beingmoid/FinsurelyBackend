
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Linq;

namespace PanoramaBackend.Services.Services
{
    public class LeaveApplicationService : BaseService<LeaveApplication, int>, ILeaveApplicationService
    {
        private readonly IVacationPolicyService _policyService;
        private readonly IEmploymentStatusService _empStatus;

        public LeaveApplicationService(RequestScope scopeContext, ILeaveApplicationRepository repo,

            IEmploymentStatusService empStatus ,
            IVacationPolicyService policyService) : base(scopeContext, repo)

        {
            _policyService = policyService;
            _empStatus = empStatus;

        }
        protected async override Task WhileInserting(IEnumerable<LeaveApplication> entities)
        {
            foreach (var item in entities)
            {
                if (item.IsSickLeave==false && item.IsAnnualLeave==false && item.IsCasualLeave==false)
                {
                    throw new ServiceException(System.Net.HttpStatusCode.NotAcceptable,"Atleast one leave should be in the application");
                }
                var empStatus = await _empStatus.GetOne(item.Id);
                var leaveCounts = (await _policyService.Get(x => x.EmploymentDetailId ==  empStatus.EmploymentDetailId 
                                && 
                                x.ForYear.Year == DateTime.Now.Year)).SingleOrDefault();
                var daysOfLeave = (item.DateOfReturn.Date - item.LastDayOfWork.Date).TotalDays;
                if (item.IsAnnualLeave)
                {
                    if (daysOfLeave> leaveCounts.AnnualLeavesCount)
                    {
                        throw new ServiceException(System.Net.HttpStatusCode.NotAcceptable, "Atleast one leave should be in the application");
                    }
                }


            }

        }
    }
    public interface ILeaveApplicationService : IBaseService<LeaveApplication, int>
    {

    }
}
