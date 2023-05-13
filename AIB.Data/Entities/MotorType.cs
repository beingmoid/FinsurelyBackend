using AIB.Common;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AIB.Data.Entities
{
    public class MotorType : BaseEntity<int>
    {
        public string Name { get; set; }

        private  ICollection<Sales> _sales;
        [JsonIgnore]
        public virtual ICollection<Sales> Sales => _sales ?? (_sales = new List<Sales>());
        private ICollection<Transaction> _transactions;
        public virtual ICollection<Transaction> Transactions => _transactions ?? (_transactions = new List<Transaction>());
    }
}
