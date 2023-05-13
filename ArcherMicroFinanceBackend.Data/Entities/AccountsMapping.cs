using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class AccountsMapping : BaseEntity<int>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string FormName { get; set; }
        public int? AccountId { get; set; }
        public Accounts Accounts { get; set; }
        public bool? isMapped { get; set; }
    }
}
