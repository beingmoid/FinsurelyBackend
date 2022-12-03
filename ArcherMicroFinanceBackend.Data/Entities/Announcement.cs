using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class Announcement:BaseEntity<int>
    {
        public string Fullname { get; set; }
        public string JobTitle { get; set; }
        public string AnnoucementTitle { get; set; }
        public DateTime Date { get; set; }
        public string  Description { get; set; }
    }
}
