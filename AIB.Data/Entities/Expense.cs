using AIB.Common;

using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Entities
{
    public class Expense:BaseEntity<int>
    {
        public int? ExpenseId { get; set; }
        public Expense ParentExpense { get; set; }
        public string ExpenseName { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal ExpenseAmount { get; set; }

        public Guid? BranchId { get; set; }
        public Branch Branch { get; set; }

        private ICollection<Expense> _expenses;
        public ICollection<Expense> Expenses => _expenses ?? (_expenses = new List<Expense>());
        private ICollection<Transaction> _transactions;
        public virtual ICollection<Transaction> Transactions => _transactions ?? (_transactions = new List<Transaction>());


    }
}
