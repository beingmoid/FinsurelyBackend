using AIB.Common;

using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Entities
{
    public class Transaction : BaseEntity<int>
    {
        public DateTime TransactionDate { get; set; }
        public int? BankId { get; set; }
        public virtual BankAccount Bank { get; set; }
        public TransactionType TransactionType { get; set; }
        public int? AgentId { get; set; }
        public virtual Agent Agent { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public decimal Amount { get; set; }
        public bool? isRefund { get; set; }
        public string CompanyName { get; set; }
        public decimal? RecevingFromBroker { get; set; }
        public decimal? GivenToAgent { get; set; }
        public int? VehicleModelId { get; set; }
        public VehicleModel VehicleModel { get; set; }

        public int? MotorTypeId { get; set; }
        public MotorType MotorType { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string PolicyNumber { get; set; }
        public int? SalesId { get; set; }
        public Sales Sales { get; set; }
        public int? ExpenseId { get; set; }
        public Expense Expense { get; set; }
        public Guid? BranchId { get; set; }
        public Branch Branch { get; set; }



    }
    public enum TransactionType
    {
        Debit=1,
        Credit
    }
}
