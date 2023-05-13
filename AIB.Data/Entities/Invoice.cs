using AIB.Common;

using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Entities
{
    public class Invoice : BaseEntity<int>
    {
        public string InvoicedTo { get; set; }
        private ICollection<Sales> _sales;
        public virtual ICollection<Sales> Sales => _sales ?? (_sales = new List<Sales>());

        private ICollection<Outstandings> _outStandings;
        public virtual ICollection<Outstandings> Outstandings => _outStandings ?? (_outStandings = new List<Outstandings>());
    }
}
