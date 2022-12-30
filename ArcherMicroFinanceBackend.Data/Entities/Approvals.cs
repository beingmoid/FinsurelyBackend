using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class Approvals:BaseEntity<int>
    {
        public bool? IsForVacations { get; set; }
        public bool? IsForLoans { get; set; }
        public int? TopMangerId { get; set; }
        public bool? IsApprovedByTopManager  { get; set; }
        public int? MidManagerId { get; set; }
        public bool? IsApprovedByMidManager { get; set; }
        public int? LineManagerId { get; set; }
        public bool? IsApprovedByLineManager { get; set; }
        public int? LoanApplicationId   { get; set; }
        public int? VacationApplicationId { get; set; }



    }
}
