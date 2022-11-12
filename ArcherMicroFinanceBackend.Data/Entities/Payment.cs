using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Entities
{
    public class Payment:BaseEntity<int>
    {
        public int? SalesAgentId { get; set; }
        public UserDetails SalesAgent { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public UserDetails InsuranceCompany { get; set; }
        public string Memo { get; set; }
        public string Email { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public int? DepositAccountId { get; set; }
        public Accounts DepositAccount { get; set; }

        public int? CreditAccountId { get; set; }
        public Accounts CreditAccount { get; set; }

        public decimal Amount { get; set; }
        public bool IsPaymentDebit { get; set; }
        public bool IsPaymentCredit{ get; set; }
        private ICollection<Transaction> _Transaction;
        public ICollection<Transaction> Transactions => _Transaction ?? (_Transaction = new List<Transaction>());



    }
}
