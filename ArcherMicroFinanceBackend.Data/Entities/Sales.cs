using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace PanoramBackend.Data.Entities
{
    public class SalesInvoice : BaseEntity<int>
    {
        public string CustomerName { get; set; }
        public string ChassisNumber { get; set; }
        public string InsuranceCompanyName { get; set; }
        public int? CustomerDetailId { get; set; }
        public UserDetails CustomerDetails { get; set; }
        public DateTime SalesInvoiceDate { get; set; }
        public int? SalesInvoicePersonId { get; set; }
        public int? BodyTypeId { get; set; }
        public BodyType BodyType { get; set; }
        public int? PolicyTypeId { get; set; }
        public PolicyType PolicyType { get; set; }

        public int? ServiceId { get; set; }
        public Service Service { get; set; }
        public UserDetails SalesInvoicePerson { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public UserDetails InsuranceCompany { get; set; }
        public int? InsuranceTypeId { get; set; }
        public InsuranceType InsuranceType { get; set; }
        public Branch Branch { get; set; }
        public int BranchId { get; set; }
        public string UnderWritter { get; set; }
        public int? PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Notes { get; set; }
        public decimal? Total { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        private ICollection<SaleLineItem> _SaleLineItem;
        //[IgnoreDataMember]
        //[JsonIgnore]
        public ICollection<SaleLineItem> SaleLineItem => _SaleLineItem ?? (_SaleLineItem = new List<SaleLineItem>());
        private ICollection<Transaction> _Transaction;
        public ICollection<Transaction> Transactions => _Transaction ?? (_Transaction = new List<Transaction>());

    }

    public enum InvoiceType
    {
        Invoice=1,
        SalesReceipt=2,
    }
    public enum PaymentStatus
    {
        Paid=1,
        Unpaid=2,
        OverDue=3,
        Closed=4,
    }
    public class PolicyType:BaseEntity<int>
    {
        public string Name { get; set; }
        private ICollection<SalesInvoice> _SalesInvoice;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> SalesInvoice => _SalesInvoice ?? (_SalesInvoice = new List<SalesInvoice>());


    }
    public class Service : BaseEntity<int> 
    {
        public int? PolicyTypeId { get; set; }
        public string Name { get; set; }
        private ICollection<SalesInvoice> _SalesInvoice;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> SalesInvoice => _SalesInvoice ?? (_SalesInvoice = new List<SalesInvoice>());

    }
    public class SaleLineItem : BaseEntity<int>
    {
        public int SaleId { get; set; }

        public SalesInvoice SalesInvoice { get; set; }
        public string PolicyNumber { get; set; }
        public int? VehilcleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public decimal Total { get; set; }
        public decimal? Gross { get; set; }
        public decimal? VAT { get; set; }
        public decimal? Commission { get; set; }
        public decimal? PremiumPrice { get; set; }
        public decimal? CommisionRate { get; set; }
        public decimal? Net { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? ActualComission { get; set; }



    }
    public class InsuranceType:BaseEntity<int>
    {
        public string Name { get; set; }
      
        private ICollection<SalesInvoice> _SalesInvoice;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> SalesInvoice => _SalesInvoice ?? (_SalesInvoice = new List<SalesInvoice>());
        private ICollection<Refund> _InsuranceTypeRefunds;
        public ICollection<Refund> InsuranceTypeRefunds => _InsuranceTypeRefunds ?? (_InsuranceTypeRefunds = new List<Refund>());

    }
    public class Vehicle : BaseEntity<int>
    {

        public string Make { get; set; }
        public string Model { get; set; }
        private ICollection<SaleLineItem> _SaleLineItem;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SaleLineItem> SaleLineItem => _SaleLineItem ?? (_SaleLineItem = new List<SaleLineItem>());
        private ICollection<Refund> _RefundsOnVehicle;
        public ICollection<Refund> RefundsOnVehicles => _RefundsOnVehicle ?? (_RefundsOnVehicle = new List<Refund>());


    }
    public class BodyType : BaseEntity<int>
    {
        public string Name { get; set; }
    
        private ICollection<SalesInvoice> _SalesInvoice;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> SalesInvoice => _SalesInvoice ?? (_SalesInvoice = new List<SalesInvoice>());
    }
}
