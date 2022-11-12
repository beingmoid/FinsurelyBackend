using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramBackend.Data.CatalogDb
{
    public partial class Databases:BaseEntity<int>
    {
        [ForeignKey("tenantId")]
        public byte[] tenantId { get; set; }
        public virtual Tenants Tenant{ get; set; }
        public string ServerName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ServiceObjective { get; set; }
        public string ElasticPoolName { get; set; }
        public string State { get; set; }
        public string RecoveryState { get; set; }
        public System.DateTime LastUpdated { get; set; }
    }
}
