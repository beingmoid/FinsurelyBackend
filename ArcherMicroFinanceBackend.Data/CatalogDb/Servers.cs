using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.CatalogDb
{
    public partial class Servers : BaseEntity<int>

    {
        public string ServerName { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public string RecoveryState { get; set; }
        public System.DateTime LastUpdated { get; set; }
    } 
}
