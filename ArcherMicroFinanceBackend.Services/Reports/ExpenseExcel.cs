using FluentExcel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PanoramaBackend.Services.Reports
{
    public class ExpenseExcel
    {
        public static void ConfigureExcel()
        {
            var fc = Excel.Setting.For<ExpenseExcel>();

            fc.Property(x => x.ExpenseDate)
                .HasExcelIndex(0)
                .HasExcelTitle("Expense Date");
            fc.Property(x => x.ExpenseCategory)
.HasExcelIndex(1)
.HasExcelTitle("Expense Category");

            fc.Property(x => x.ExpenseName)
    .HasExcelIndex(2)
    .HasExcelTitle("Expense Name");

            fc.Property(x => x.Branch)
    .HasExcelIndex(3)
    .HasExcelTitle("Branch");

            fc.Property(x => x.ExpenseAccount)
.HasExcelIndex(4)
.HasExcelTitle("Expense Account");


            fc.Property(x => x.ExpenseAccount)
.HasExcelIndex(5)
.HasExcelTitle("Expense Amount");






        }

        public string ExpenseName { get; set; }
        public string ExpenseDate { get; set; }
        public string ExpenseCategory { get; set; }
        public string Branch { get; set; }
        public string ExpenseAmount { get; set; }
        public string ExpenseAccount { get; set; }

    }
}
