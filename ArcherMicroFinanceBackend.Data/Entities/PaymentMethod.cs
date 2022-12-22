using NukesLab.Core.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PanoramBackend.Data.Entities
{
    public class PaymentMethod:BaseEntity<int>
    {
        public string Name { get; set; }
        private ICollection<SalesInvoice> _SalesInvoice;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> SalesInvoice => _SalesInvoice ?? (_SalesInvoice = new List<SalesInvoice>());
   
        private ICollection<Payment> _AgentPayment;
        public ICollection<Payment> Payments => _AgentPayment ?? (_AgentPayment = new List<Payment>());

    }
}