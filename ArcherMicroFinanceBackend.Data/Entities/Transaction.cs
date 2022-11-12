using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Entities
{
    public class Transaction:BaseEntity<int>
    {
        public DateTime TransactionDate { get; set; }
        public string Memo { get; set; }
        public int? UserDetailId { get; set; } //SaleAgent 
        public int? SalesInvoiceId { get; set; } //Invoice
        public int? PaymentId { get; set; } //ReceivePayment
        public Payment Payment { get; set; }

        public int? RefundId { get; set; } //Refund
        public Refund Refund { get; set; }
        public TransactionTypes? TransactionType { get; set; }
        public SalesInvoice SalesInvoice { get; set; }
        public UserDetails UserDetails { get; set; }
        private ICollection<LedgarEntries> _ledger;
        public ICollection<LedgarEntries> LedgarEntries => _ledger ?? (_ledger = new List<LedgarEntries>());
    }
    
    public enum TransactionTypes :int
    {
     
        Invoice=1,
        Payment,

        InsuranceCredit,
        Transfer,
        Deposit,
        Expense,
        Bill,
        Refund

    }
}
