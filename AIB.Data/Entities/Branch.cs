using AIB.Common;
using Castle.Components.DictionaryAdapter;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AIB.Data.Entities
{
    public class Branch:BaseEntity<Guid>
    {
        [Key("Id")]
        public override Guid Id { get;set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal? Lng { get; set; }
        public decimal? Lat { get; set; }              
        public bool MainBranch { get; set; }

        private ICollection<Agent> _agents;
        public ICollection<Agent> Agents => _agents ?? (_agents = new List<Agent>());
        private ICollection<ExtendedUser> _manager;
        public ICollection<ExtendedUser> Managers => _manager ?? (_manager = new List<ExtendedUser>());

        private ICollection<Transaction> _transactions;
        public ICollection<Transaction> Transactions => _transactions ?? (_transactions = new List<Transaction>());

        private ICollection<Sales> _sales;
        [JsonIgnore]
        public ICollection<Sales> Sales => _sales ?? (_sales = new List<Sales>());
        private ICollection<Expense> _expense;
        public ICollection<Expense> Expenses => _expense ?? (_expense = new List<Expense>());


    }
}
