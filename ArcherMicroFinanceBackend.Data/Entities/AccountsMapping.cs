using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Entities
{
    public class AccountsMapping : BaseEntity<int>
    {

        public string FormName { get; set; }
        public int? AccountId { get; set; }
        public Accounts Accounts { get; set; }
        public bool? isMapped { get; set; }
    }
}
