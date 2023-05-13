using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using PanoramaBackend.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace PanoramaBackend.Data.Entities
{
    public class Branch : BaseEntity<Guid>
    {
        [Key]
        public override Guid Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string BranchName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string BranchAddress { get; set; }
     
        private ICollection<SalesInvoice> _sales;

        public ICollection<SalesInvoice> Sales => _sales ?? (_sales = new List<SalesInvoice>());

        private ICollection<Transaction> _Transactions;

        public ICollection<Transaction> Transaction => _Transactions ?? (_Transactions = new List<Transaction>());
        private ICollection<Expense> _expenseByBranch;
        public ICollection<Expense> ExpensesByBranch => _expenseByBranch ?? (_expenseByBranch = new List<Expense>());

        private ICollection<Payroll> _paidToBranch;
        public ICollection<Payroll> PaidToBranch => _paidToBranch ?? (_paidToBranch = new List<Payroll>());

        private ICollection<VacationApplication> _applications;
        public ICollection<VacationApplication> Vacations => _applications ?? (_applications = new List<VacationApplication>());

    }
}
