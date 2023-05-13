using AIB.Common;

using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Entities
{
    public class BankAccount : BaseEntity<int>
    {
        public string BankName { get; set; }
        public string BankShortName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBANNo { get; set; }
        public string BankLogoUrl { get; set; }
        public string BranchName { get; set; }
        public string SWIFTCode { get; set; }
        public decimal Amount { get; set; }
        public AccountType AccountType { get; set; }

        private ICollection<Transaction> _transactions;
        public virtual ICollection<Transaction> Transactions => _transactions ?? (_transactions = new List<Transaction>());
    }
    public enum AccountType
    {
        Current=1,
        Saving
    }
}
