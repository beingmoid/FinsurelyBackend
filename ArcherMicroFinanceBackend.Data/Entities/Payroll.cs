using NukesLab.Core.Common;
using PanoramaBackend.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class Payroll:BaseEntity<int>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PayrollStatus Status { get; set; }
        public int ExpenseAccountId { get; set; }
        public Accounts ExpenseAccount { get; set; }
        public bool IsRecurring { get; set; }


    }
    public enum PayrollStatus
    {
        Active=1,
        Deactive=2,

    }
}
