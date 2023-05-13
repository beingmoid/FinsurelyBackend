using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class Attendance :BaseEntity<int>
    {
        public int EmployeeId { get; set; }
        public EmploymentDetails EmploymentDetails { get; set; }
        public DateTime Date { get; set; }    
    }
}
