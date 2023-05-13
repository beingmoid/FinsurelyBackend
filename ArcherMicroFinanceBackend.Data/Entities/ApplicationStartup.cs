using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class ApplicationStartup :BaseEntity<Guid>
    {

        public override Guid Id { get; set; }
        public Guid Token { get; set; }

        public string Name { get; set; }
        public bool Expired { get; set; }
        
    }
}
