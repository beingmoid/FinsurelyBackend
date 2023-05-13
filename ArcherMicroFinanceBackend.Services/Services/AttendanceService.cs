using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PanoramaBackend.Services.Services
{
    public class AttendanceService : BaseService<Attendance, int>, IAttendanceService
    {
        private readonly IEmployeeDetailsRepository _empService;
        private readonly IVacationPolicyService _vacationPolicyService;
        private readonly IBenefitsAndDeductionService _deductionService;
        private readonly ILeaveApplicationService _leaves;

        public AttendanceService(RequestScope scopeContext, IAttendanceRepository repo,
            IVacationPolicyService vacationPolicyService,
            IBenefitsAndDeductionService deductionService,
            ILeaveApplicationService leaves
            ,IEmployeeDetailsRepository empService) : base(scopeContext, repo)
        {
            _empService = empService;
            _vacationPolicyService = vacationPolicyService;
            _deductionService = deductionService;
            _leaves = leaves;
        }
        public async Task ProcessAbsentees()
        {
            var attendance = (await this.Get(x => x.Date == DateTime.Now)).Select(x=> x.EmployeeId);
            var employees = await _empService.Get(x=>x.Include(x=>x.VacationPolicies).Include(x=>x.Compensations)
            .Include(x=>x.EmploymentStatus).ThenInclude(x=>x.LeaveApplications)
            ,x => x.EmployeeIsActive);
            var absentes = new List<EmploymentDetails>();
            foreach (var item in employees)
            {
                if (!attendance.Contains(item.Id))
                {
                    absentes.Add(item);
                }
            }

            foreach (var item in absentes)
            {
                if (item.VacationPolicies.Count>0)
                {
                    LeaveApplication leave = null;
                    var policy = item.VacationPolicies.SingleOrDefault(x => x.ForYear.Year == DateTime.Now.Year);
                    foreach (var status in item.EmploymentStatus)
                    {
                        if (status.LeaveApplications.Any(x=>x.LeaveIsActive))
                        {
                            leave = status.LeaveApplications.SingleOrDefault(x => x.LeaveIsActive);
                            break;
                        }
                    }
                    if (policy != null && leave.IsSickLeave)
                    {
                        if (policy.SickLeaveCount > 0)
                        {
                            policy.SickLeaveCount -= 1;
                            await _vacationPolicyService.Update(policy.Id, policy);
                        }
                        else
                        {
                            var deduction = new BenefitsAndDeduction();
                            deduction.ApplicableDate = DateTime.Now;
                            deduction.EmploymentDetailId = item.Id;
                            var salaryForOneDay = item.Compensations.SingleOrDefault(x => x.Effective).SalaryAmount / DateTime.DaysInMonth(DateTime.Now.Month, DateTime.Now.Year);
                            deduction.Deduction.Add(new Deduction()
                            {
                                DeductionTypeId = 1,
                                PayStubLabel = $"Deduction for leave taken on Date of {DateTime.Now.ToShortDateString()}."
                                ,
                                Amount = salaryForOneDay,
                                Occurance = Occurance.Once
                            });
                            await _deductionService.Insert(new[] { deduction });
                        }
                    }
                   else if (policy!=null && leave.IsCasualLeave)
                    {
                        if (policy.CasualLeaveCount>0)
                        {
                            policy.CasualLeaveCount -= 1;
                           await _vacationPolicyService.Update(policy.Id, policy);
                        }
                        else
                        {
                            var deduction = new BenefitsAndDeduction();
                            deduction.ApplicableDate = DateTime.Now;
                            deduction.EmploymentDetailId = item.Id;
                            var salaryForOneDay = item.Compensations.SingleOrDefault(x => x.Effective).SalaryAmount/DateTime.DaysInMonth(DateTime.Now.Month,DateTime.Now.Year);
                            deduction.Deduction.Add(new Deduction()
                            {
                                DeductionTypeId=1,
                                PayStubLabel=$"Deduction for leave taken on Date of {DateTime.Now.ToShortDateString()}."
                                ,Amount=salaryForOneDay,
                                Occurance=Occurance.Once
                            });
                           await _deductionService.Insert(new[] { deduction });
                        }
                    
                    }
                    else if (policy != null && leave.IsAnnualLeave)
                    {
                        if (policy.AnnualLeavesCount > 0)
                        {
                            policy.AnnualLeavesCount -= 1;
                            await _vacationPolicyService.Update(policy.Id, policy);
                        }
                        else
                        {
                            var deduction = new BenefitsAndDeduction();
                            deduction.ApplicableDate = DateTime.Now;
                            deduction.EmploymentDetailId = item.Id;
                            var salaryForOneDay = item.Compensations.SingleOrDefault(x => x.Effective).SalaryAmount / DateTime.DaysInMonth(DateTime.Now.Month, DateTime.Now.Year);
                            deduction.Deduction.Add(new Deduction()
                            {
                                DeductionTypeId = 1,
                                PayStubLabel = $"Deduction for leave taken on Date of {DateTime.Now.ToShortDateString()}."
                                ,
                                Amount = salaryForOneDay,
                                Occurance = Occurance.Once
                            });
                            await _deductionService.Insert(new[] { deduction });
                        }

                    }
                    else
                    {
                        var deduction = new BenefitsAndDeduction();
                        deduction.ApplicableDate = DateTime.Now;
                        deduction.EmploymentDetailId = item.Id;
                        var salaryForOneDay = item.Compensations.SingleOrDefault(x => x.Effective).SalaryAmount / DateTime.DaysInMonth(DateTime.Now.Month, DateTime.Now.Year);
                        deduction.Deduction.Add(new Deduction()
                        {
                            DeductionTypeId = 1,
                            PayStubLabel = $"Deduction for leave taken on Date of {DateTime.Now.ToShortDateString()}."
                            ,
                            Amount = salaryForOneDay,
                            Occurance = Occurance.Once
                        });
                        await _deductionService.Insert(new[] { deduction });
                    }
                }
            }
        }
    }
    public interface IAttendanceService : IBaseService<Attendance, int>
    {
        Task ProcessAbsentees();
    }
}
