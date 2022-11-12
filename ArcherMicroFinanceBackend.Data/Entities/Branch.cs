using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace PanoramBackend.Data.Entities
{
    public class Branch : BaseEntity<int> 
    {
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
     
        private ICollection<SalesInvoice> _sales;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> Sales => _sales ?? (_sales = new List<SalesInvoice>());

    }
}
