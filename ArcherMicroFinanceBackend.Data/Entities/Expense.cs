using NukesLab.Core.Common;
using PanoramBackend.Data.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class Expense:BaseEntity<int>
    {
        public string ExpenseName { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public decimal ExpenseAmount { get; set; }
        public int AccountId { get; set; }
        public Accounts Account { get; set; }

        private ICollection<Transaction> _Transaction;
        public ICollection<Transaction> Transactions => _Transaction ?? (_Transaction = new List<Transaction>());
   
    }
    public class ExpenseCategory:BaseEntity<int>
    {
        public string Name { get; set; }
    }
}
