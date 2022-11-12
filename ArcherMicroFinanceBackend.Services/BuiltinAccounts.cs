using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services
{
    public static class BuiltinAccounts
    {
        public static int CashAccount => 1;
        public static int AccountsRecievable => 3;
        public static int AccountsPayable => 4;
        public static int ExpenseAccount => 5;
        public static int SalesAccount => 6;
        public static int VATPayable => 7;
        public static int RetainedEarning => 8;
        public static int OpeningBalanceEquity => 9;
        public static int RefundIncome => 100;
        public static int ComissionIncome => 100;

    }
}
