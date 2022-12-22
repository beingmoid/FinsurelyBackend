using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services
{
    public class ExpenseReport
    {
        public string ExpenseName { get; set; }
        public string ExpenseDate { get; set; }
        public string ExpenseCategory { get; set; }
        public string ExpenseAmount { get; set;}
        public string ExpenseAccount { get; set; }
    }
}
