
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class SetupClient : BaseEntity<int>
    {
        public bool FirstTime { get; set; }

        

    }

}
