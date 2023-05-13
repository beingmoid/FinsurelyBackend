using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class Announcement:BaseEntity<int>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Fullname { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string JobTitle { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string AnnoucementTitle { get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string  Description { get; set; }
    }
}
