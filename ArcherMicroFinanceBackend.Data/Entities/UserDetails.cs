using PanoramaBackend.Services.Data.DTOs;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using PanoramaBackend.Data.Repository;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using PanoramaBackend.Data.Entities;

namespace PanoramaBackend.Data.Entities
{
    public class UserDetails:BaseEntity<int>
    {

      
        public bool? IsCustomer { get; set; }
        public bool? IsEmployee { get; set; }
        public bool? IsSupplier { get; set; }
        public bool? IsInsuranceCompany { get; set; }
        public bool? IsAgent { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string MiddleName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Suffix { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Company { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string DisplayNameAs { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Phone { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Mobile { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Fax { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Other { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Website { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public bool? BillWithParent { get; set; }
        public bool? IsSubCustomer { get; set; }
      
        public int? UserDetailId { get; set; }
        public string ImageUrl { get; set; } //Logo Or Avatar
        public UserDetails UserDetail { get; set; }
        public Guid UserId { get; set; }
        public virtual ExtendedUser ExtendedUser { get; set; }
        public decimal? OpenBalance { get; set; }


        public int? DefaultAccountId { get; set; }
        [ForeignKey("DefaultAccountId")]
        public Accounts Accounts { get; set; }
        private ICollection<ComissionRate> _comissionRates;
        public ICollection<ComissionRate> ComissionRates => _comissionRates ?? (_comissionRates = new List<ComissionRate>());
        private ICollection<UserDetails> _userDetails;
        public ICollection<UserDetails> Parent => _userDetails ?? (_userDetails=new List<UserDetails>());
        private ICollection<Transaction> _Transaction;
        [IgnoreDataMember] 
        [JsonIgnore]
        public ICollection<Transaction> Transactions => _Transaction ?? (_Transaction = new List<Transaction>());

        private ICollection<Attachments> _Attachments;
        public ICollection<Attachments> Attachments => _Attachments ?? (_Attachments = new List<Attachments>());

        private ICollection<SalesInvoice> _insuranceCompanyInvoices;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> InsuranceCompanyInvoices => _insuranceCompanyInvoices ?? (_insuranceCompanyInvoices = new List<SalesInvoice>());

        private ICollection<SalesInvoice> _SalesInvoicePersons;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> SalesInvoicePersons => _SalesInvoicePersons ?? (_SalesInvoicePersons = new List<SalesInvoice>());
        private ICollection<SalesInvoice> _CustomerSalesInvoice;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> CustomerSalesInvoice => _CustomerSalesInvoice ?? (_CustomerSalesInvoice = new List<SalesInvoice>());
        
        private ICollection<Address> _Address;
        
        public ICollection<Address> Addresses => _Address ?? (_Address = new List<Address>());

        private ICollection<PaymentAndBilling> _PaymentAndBilling;
        public ICollection<PaymentAndBilling> PaymentAndBilling => _PaymentAndBilling ?? (_PaymentAndBilling = new List<PaymentAndBilling>());


        private ICollection<Payment> _AgentPayment;
        public ICollection<Payment> Payments => _AgentPayment ?? (_AgentPayment = new List<Payment>());
        private ICollection<Payment> _InsuranceCompanyPayment;
        public ICollection<Payment> InsuranceCompanyPayment => _InsuranceCompanyPayment ?? (_InsuranceCompanyPayment = new List<Payment>());
        private ICollection<Refund> _AgentRefunds;
        public ICollection<Refund> AgentRefunds => _AgentRefunds ?? (_AgentRefunds = new List<Refund>());

        private ICollection<Refund> _InsuranceCompanyRefunds;
        public ICollection<Refund> InsuranceCompanyRefunds => _InsuranceCompanyRefunds ?? (_InsuranceCompanyRefunds = new List<Refund>());
        private ICollection<Reconcilation> _ReconcilationsAgents;
        public ICollection<Reconcilation> ReconcilationAgents => _ReconcilationsAgents ?? (_ReconcilationsAgents = new List<Reconcilation>());
        private ICollection<Reconcilation> _ReconcilationInsuranceCompany;
        public ICollection<Reconcilation> ReconcilationInsuranceCompany => _ReconcilationInsuranceCompany ?? (_ReconcilationInsuranceCompany = new List<Reconcilation>());

        private ICollection<EmploymentDetails> _EmploymentDetails;
        public ICollection<EmploymentDetails> EmploymentDetails => _EmploymentDetails ?? (_EmploymentDetails = new List<EmploymentDetails>());
        private ICollection<EmploymentDetails> _ManagerResources;
        public ICollection<EmploymentDetails> ManagerResources => _ManagerResources ?? (_ManagerResources = new List<EmploymentDetails>());

        private ICollection<Teams> _ManagerTeams;
        public ICollection<Teams> ManagerTeams => _ManagerTeams ?? (_ManagerTeams = new List<Teams>());
        private ICollection<TaskTodo> _task;
        public virtual ICollection<TaskTodo> AssignedTask => _task ?? (_task = new List<TaskTodo>());
        private ICollection<TaskTodo> task;
        public virtual ICollection<TaskTodo> AssignedByTask => task ?? (task = new List<TaskTodo>());


        private ICollection<VacationApplication> _applications;
        public ICollection<VacationApplication> Vacations => _applications ?? (_applications = new List<VacationApplication>());

    }
    public class Address:BaseEntity<int>
    {
        public int? UserDetailId { get; set; }
        public UserDetails UserDetails { get; set; }
        public string BillingAddress { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

    }
    public class PaymentAndBilling : BaseEntity<int>
    {
        public int? UserDetailId { get; set; }
        public UserDetails UserDetails { get; set; }
        public int? PreferredPaymentMethodId { get; set; }
        public PreferredPaymentMethod PreferredPaymentMethod { get; set; }
        public PreferredDeliveryMethod PreferredDeliveryMethod { get; set; }
        public int? TermsId { get; set; }
        public Terms Terms { get; set; }
        public decimal? OpeningBalance { get; set; }
        public DateTime? Asof { get; set; }

    }
    public class Attachments : BaseEntity<int>
    {
        public int? UserDetailId { get; set; }
        public UserDetails UserDetails { get; set; }
        public string AttachmentUrl { get; set; }
    }
    public class PreferredPaymentMethod : BaseEntity<int>
    {
        public string Text { get; set; }

        private ICollection<PaymentAndBilling> _PaymentAndBilling;
        public ICollection<PaymentAndBilling> PaymentAndBilling => _PaymentAndBilling ?? (_PaymentAndBilling = new List<PaymentAndBilling>());

    }
    public class Terms: BaseEntity<int>
    {
        public string Text { get; set; }
        private ICollection<PaymentAndBilling> _PaymentAndBilling;
        public ICollection<PaymentAndBilling> PaymentAndBilling => _PaymentAndBilling ?? (_PaymentAndBilling = new List<PaymentAndBilling>());
    }
        public enum PreferredDeliveryMethod
        {
            None,
            PrintLater,
            SendLater
        }
}
