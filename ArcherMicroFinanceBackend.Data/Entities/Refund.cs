using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class Refund:BaseEntity<int>
    {
        public int? AgentId { get; set; }
        public UserDetails Agent { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public UserDetails InsuranceCompany { get; set; }
        public DateTime RefundDate { get; set; }
        //public int PaymentMethodId { get; set; }
        //public PaymentMethod PaymentMethod { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string CompanyName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string CustomerName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string PolicyNumber { get; set; }
        public int? InsuranceTypeId { get; set; }
        public InsuranceType InsuranceType { get; set; }
        public int? VehilcleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public string MessageOnReceipt { get; set; }
        public string MessageOnStatement { get; set; }
        public decimal AmountForSalesAgent { get; set; }
        public decimal AmountForBroker { get; set; }

        private ICollection<Transaction> _Transaction;
        public ICollection<Transaction> Transactions => _Transaction ?? (_Transaction = new List<Transaction>());
    }
}
