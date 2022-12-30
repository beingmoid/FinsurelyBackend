using NukesLab.Core.Common;
using PanoramBackend.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class VacationApplication:BaseEntity<int>
    {
        public int UserDetailId { get; set; }
        public UserDetails EmployeeDetails { get; set; }
        public string Description { get; set; }
        public string ApplicantName { get; set; }
        public string JobTitle { get; set; }
        public string VacationTitle { get; set; }
        public string VacationDescription { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }



    }
}
