using NukesLab.Core.Common;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Entities
{
    public class Refund:BaseEntity<int>
    {
        public int? AgentId { get; set; }
        public UserDetails Agent { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public UserDetails InsuranceCompany { get; set; }
        public DateTime RefundDate { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int? PolicyTypeId { get; set; }
        public PolicyType PolicyType { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }

        public string PolicyNumber { get; set; }
        public int? InsuranceTypeId { get; set; }
        public InsuranceType InsuranceType { get; set; }
        public int? VehilcleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public string MessageOnReceipt { get; set; }
        public string MessageOnStatement { get; set; }
        public decimal AmountForSalesAgent { get; set; }
        public decimal AmountForBroker { get; set; }
        public int AccountId { get; set; }
        public Accounts Account { get; set; }
        private ICollection<Transaction> _Transaction;
        public ICollection<Transaction> Transactions => _Transaction ?? (_Transaction = new List<Transaction>());
    }
}
