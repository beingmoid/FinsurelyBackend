using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Entities
{

    public class Teams : BaseEntity<int>
    {
        public int ManagerId { get; set; }
        public UserDetails Manager { get; set; }
        public string TeamName { get; set; }

        private ICollection<EmploymentDetails> _EmploymentDetails;
        public ICollection<EmploymentDetails> TeamMembers => _EmploymentDetails ?? (_EmploymentDetails = new List<EmploymentDetails>());

    }

    public class EmploymentDetails:BaseEntity<int>
    {
        public int? ManagerId { get; set; }
        public UserDetails Manager { get; set; }
        public int UserDetailId { get; set; }
        public UserDetails UserDetails { get; set; }
        public string Position { get; set; }
        public DateTime HiredDate { get; set; }
        public int? TeamId { get; set; }
        public Teams Team { get; set; }
        public bool EmployeeIsActive { get; set; }
        private ICollection<Compensation> _compensation;
        public ICollection<Compensation> Compensations => _compensation ?? (_compensation = new List<Compensation>());
        private ICollection<VacationPolicy> _VacationPolicy;
        public ICollection<VacationPolicy> VacationPolicies => _VacationPolicy ?? (_VacationPolicy = new List<VacationPolicy>());
        private ICollection<BenefitsAndDeduction> _BenefitsAndDeduction;
        public ICollection<BenefitsAndDeduction> BenefitsAndDeductions => _BenefitsAndDeduction ?? (_BenefitsAndDeduction = new List<BenefitsAndDeduction>());
        private ICollection<EmployeeFiles> _EmployeeFiles;
        public ICollection<EmployeeFiles> EmployeeFiles => _EmployeeFiles ?? (_EmployeeFiles = new List<EmployeeFiles>());
        private ICollection<BankDetails> _BankDetails;
        public ICollection<BankDetails> BankDetails => _BankDetails ?? (_BankDetails = new List<BankDetails>());
        private ICollection<EmploymentStatus> _EmploymentStatus;
        public ICollection<EmploymentStatus> EmploymentStatus => _EmploymentStatus ?? (_EmploymentStatus = new List<EmploymentStatus>());

    }
    public class Compensation:BaseEntity<int>
    {
        public int EmploymentDetailId { get; set; }
        public EmploymentDetails EmploymentDetails { get; set; }
        public decimal SalaryAmount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool Effective { get; set; }
        public bool? Expired { get; set; }

    }
    public class VacationPolicy:BaseEntity<int>
    {
        public int EmploymentDetailId { get; set; }
        public EmploymentDetails EmploymentDetails { get; set; }
        public int AnnualLeavesCount { get; set; }
        public int CasualLeaveCount { get; set; }
        public int SickLeaveCount { get; set; }
        public DateTime ForYear { get; set; }
        public bool Applicable { get; set; }


    }
    public class BenefitsAndDeduction : BaseEntity<int>
    {
        public int EmploymentDetailId { get; set; }
        public EmploymentDetails EmploymentDetails { get; set; }
        public DateTime? ApplicableDate { get; set; }
        public bool Applied { get; set; }
        private ICollection<Benefits> _benefits;
        public ICollection<Benefits> Benefits => _benefits ?? (_benefits= new List<Benefits>());

        private ICollection<Deduction> _Deduction; 
        public ICollection<Deduction> Deduction => _Deduction ?? (_Deduction = new List<Deduction>());

    }
    public class Benefits : BaseEntity<int>
    {
        public int BenefitAndDeductionId { get; set; }
        public BenefitsAndDeduction BenefitsAndDeduction { get; set; }
        public int BenefitTypeId { get; set; }
        public string PayStubLabel { get; set; }
        public decimal Amount { get; set; }
        //public int TypeId { get; set; }
        public BDType Type { get; set; }
        public Occurance Occurance { get; set; }
    }
    public class Deduction : BaseEntity<int>
    {
        public int BenefitAndDeductionId { get; set; }
        public BenefitsAndDeduction BenefitsAndDeduction { get; set; }
        public int DeductionTypeId { get; set; }
        public string PayStubLabel { get; set; }
        public decimal Amount { get; set; }
        //public int TypeId { get; set; }
        public BDType Type { get; set; }
        public Occurance Occurance { get; set; }
    }
    public class BDType:BaseEntity<int>
    {
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public BDType Category { get; set; }
        public bool? IsForBenefit { get; set; }
        public bool? IsForDeduction { get; set; }
        private ICollection<BDType> _benefitTypes;
        public ICollection<BDType> ChildernTypes => _benefitTypes ?? (_benefitTypes = new List<BDType>());
        private ICollection<Deduction> _Deduction;
        public ICollection<Deduction> Deductions => _Deduction ?? (_Deduction = new List<Deduction>());
        private ICollection<Benefits> _Benefits;
        public ICollection<Benefits> Benefits => _Benefits ?? (_Benefits = new List<Benefits>());


    }
    public class EmployeeFiles : BaseEntity<int>
    {
        public int EmploymentDetailId { get; set; }
        public EmploymentDetails EmploymentDetails { get; set; }
        public string DocumentUrl { get; set; }
        public string Description { get; set; }
    }
    public class BankDetails:BaseEntity<int>
    {
        public int EmploymentDetailId { get; set; }
        public EmploymentDetails EmploymentDetails { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string ibanNumber { get; set; }
        public BankType BankType { get; set; }

    }
    public class EmploymentStatus:BaseEntity<int>
    {
        public int EmploymentDetailId { get; set; }
        public EmploymentDetails EmploymentDetails { get; set; }
        private ICollection<StaffOffBoarding> staffOffBoardings;
        public ICollection<StaffOffBoarding> StaffOffBoardings => staffOffBoardings ?? (staffOffBoardings = new List<StaffOffBoarding>());
        private ICollection<LeaveApplication> leaveApplications;
        public ICollection<LeaveApplication> LeaveApplications => leaveApplications ?? (leaveApplications = new List<LeaveApplication>());

    }
    public class StaffOffBoarding : BaseEntity<int>
    {

        public int EmploymentStatusId { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public string Reason { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public DateTime DateOfNotice { get; set; }

    }
    public class LeaveApplication:BaseEntity<int>
    {
        public int EmploymentStatusId { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public bool IsSickLeave { get; set; }
        public bool IsCasualLeave { get; set; }
        public bool IsAnnualLeave { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public DateTime DateOfReturn { get; set; }
        public bool? LeaveAppoved { get; set; }
        public bool? LeaveRejected { get; set; }
        public string LeaveNoteFromEmployee { get; set; }
        public bool LeaveIsActive { get; set; }


    }

    public enum BankType
    {
        Current=1,
        Saving=2
    }
    public enum Occurance
    {
      Yearly=1,
      Monthly,
      Quater,
      Once
    }
}
