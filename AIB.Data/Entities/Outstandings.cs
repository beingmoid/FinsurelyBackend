using AIB.Common;

using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Entities
{
    public class Outstandings : BaseEntity<int>
    {

        public decimal Amount { get; set; }
        public virtual Company Company { get; set; }
        public int? CompanyId { get; set; }
        public DateTime DateOfOutstanding { get; set; }
        public virtual Agent Agent { get; set; }
        public int? AgentId { get; set; }
        public int? SalesId { get; set; }
        public virtual Sales Sales { get; set; }
        public int? TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public int? InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }

    }
}
