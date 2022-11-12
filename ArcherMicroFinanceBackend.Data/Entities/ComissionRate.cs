using NukesLab.Core.Common;
using PanoramBackend.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class ComissionRate:BaseEntity<int>
    {
        public int UserDetailId { get; set; }
        public UserDetails UserDetail { get; set; }
        public bool IsTpl { get; set; }
        public bool IsNonTpl { get; set; }
        public decimal Rates { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? ExpiredDate { get; set; }        

    }
}
