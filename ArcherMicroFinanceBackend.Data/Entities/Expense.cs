using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using PanoramaBackend.Data.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class Expense:BaseEntity<int>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string ExpenseName { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public decimal ExpenseAmount { get; set; }
        public int AccountId { get; set; }
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }
        public Accounts Account { get; set; }

        private ICollection<Transaction> _Transaction;
        public ICollection<Transaction> Transactions => _Transaction ?? (_Transaction = new List<Transaction>());
   
    }
    public class ExpenseCategory:BaseEntity<int>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
    }
}
