using Nest;
using System.Security.Policy;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace PanoramaBackend.Api.Controllers
{
    public class PageConfig
    {
        public PageConfig()
        {
            this.Data = new List<dynamic>();
        }
        public string From { get; set; }
        public string To { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<dynamic> Data { get; set; }

    }
    public class AgentStatementDTO
    {
        public int Num { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyType { get; set; }
        public string ServiceType { get; set; }
        public string InsuranceType { get; set; }
        public string Vehicle { get; set; }
        public string BodyType { get; set; }
        public string RefNo { get; set; }
        public dynamic  Debit { get; set; }
        public dynamic Credit { get; set; }
        public dynamic Balance { get; set; }

        public PageConfig PageConfig { get; set; }
    }
}
