using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Entities
{
    public class LedgarEntries:BaseEntity<int>
    {
        public decimal Amount { get; set; }
        public int? DebitAccountId { get; set; }
        public Accounts DebitAccount { get; set; }
        public int? CreditAccountId { get; set; }
        public Accounts CreditAccount { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public DateTime TransactionDate { get; set; }


    }
}
