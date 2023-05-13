using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PanoramaBackend.Data.Entities
{
    public class PaymentMethod:BaseEntity<int>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        private ICollection<SalesInvoice> _SalesInvoice;
        [IgnoreDataMember]
        [JsonIgnore]
        public ICollection<SalesInvoice> SalesInvoice => _SalesInvoice ?? (_SalesInvoice = new List<SalesInvoice>());
   
        private ICollection<Payment> _AgentPayment;
        public ICollection<Payment> Payments => _AgentPayment ?? (_AgentPayment = new List<Payment>());

    }
}