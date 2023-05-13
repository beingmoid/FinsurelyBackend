using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using PanoramaBackend.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class VacationApplication:BaseEntity<int>
    {
        public int UserDetailId { get; set; }
        public UserDetails EmployeeDetails { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }
        [Column(TypeName = "nvarchar(1200)")]
        public string ApplicantName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string JobTitle { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string VacationTitle { get; set; }
        [Column(TypeName = "nvarchar(1200)")]
        public string VacationDescription { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }



    }
}
