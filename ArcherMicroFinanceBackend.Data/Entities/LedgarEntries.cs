using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class LedgarEntries:BaseEntity<int>
    {
        public decimal Amount { get; set; }
        public int? DebitAccountId { get; set; }
        public Accounts DebitAccount { get; set; }
        public int? CreditAccountId { get; set; }
        public Accounts CreditAccount { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string EntryNote { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public DateTime TransactionDate { get; set; }


    }
}
