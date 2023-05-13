using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class Reconcilation:BaseEntity<int>
    {
        public int? SalesAgentId { get; set; }
        public UserDetails SalesAgent { get; set; }
        public int DocumentId { get; set; }
        public Documents Documents { get; set; }
        public DateTime GeneratedDate { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal AmountDifference { get; set; }
        public int NoOfSalesMissing { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public UserDetails InsuranceCompany { get; set; }
        private ICollection<CompanyInformation> _corrections;
        public ICollection<CompanyInformation> Corrections => _corrections ?? (_corrections = new List<CompanyInformation>());
    }
    public class CompanyInformation:BaseEntity<int>
    {
        public int ReconcilationReportId { get; set; }
        public Reconcilation Reconcilation { get; set; }
        public int? TempId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(1200)")]
        public string Description { get; set; }


    }
    public class Documents : BaseEntity<int>
    {
        public string BlobFileName { get; set; }
        public string BlobURI { get; set; }
        private ICollection<Reconcilation> _ReconcilationInsuranceCompany;
        public ICollection<Reconcilation> ReconcilationInsuranceCompany => _ReconcilationInsuranceCompany ?? (_ReconcilationInsuranceCompany = new List<Reconcilation>());
    }
    public enum CorrectionType : int
    {
        InsuranceType,
        Vehicle,
        Amount,
        InvoiceDate
    }
}
