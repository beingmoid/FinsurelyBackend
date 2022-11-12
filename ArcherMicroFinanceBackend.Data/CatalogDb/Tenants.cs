using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramBackend.Data.CatalogDb
{
    public class Tenants:BaseEntity<byte[]>
    {
        public override byte[] Id { get; set ; }
        public string TenantName { get; set; }
        public string ServicePlan { get; set; }
        public string RecoveryState { get; set; }
        public System.DateTime LastUpdated { get; set; }

        public int companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company Company { get; set; }
    }
}
