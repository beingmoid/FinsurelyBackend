using AIB.Common;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AIB.Data.Entities
{
    public class Sales : BaseEntity<int>
    {
        public string PolicyNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string ChassisNumber { get; set; }
   

        public DateTime? YearOfManufacturing { get; set; }
        public decimal Commission { get; set; }
        public decimal NETPrice { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal PremiumPrice { get; set; }
        public DateTime SalesDate { get; set; }
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public int VehicleModelId { get; set; }

        public virtual VehicleModel VehicleModel { get; set; }
        public int MotorTypeId { get; set; }
   
        public virtual MotorType MotorType { get; set; }
        public int SalesAgentId { get; set; }

        public virtual Agent SalesAgent { get; set; }
        public int? InvoiceId { get; set; }
 
        public virtual Invoice Invoice { get; set; }
        public int BrokerId { get; set; }
   
        public Broker Broker { get; set; }
        public Guid? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
        public PaymentStatus Status { get; set; }
        public string PolicyIssuer { get; set; }
        public string Remark { get; set; }
        public decimal? SaleCommission { get; set; }

    
        private ICollection<Outstandings> _outStandings;
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Outstandings> Outstandings => _outStandings ?? (_outStandings = new List<Outstandings>());
        private ICollection<Transaction> _transactions;
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions => _transactions ?? (_transactions = new List<Transaction>());


    }
    public enum PaymentStatus
    {
        Pending=1,
        Paid=2,
    }
}
