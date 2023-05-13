
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public class UserCompanyInformation : BaseEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]

        public string LegalStructure { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string EmailAddress { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Website { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ContactInformation { get; set; }
        public string  LogoUrl { get; set; }
        public string LogoBase64 { get; set; }
        public DateTime? PayrollDate { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string VATRegistrationNumber { get; set; }
        public bool isMigrationRequired { get; set; }

        [Column("nvarchar(100)")]
        public string OrganizationName { get; set; }
        [Column("nvarchar(100)")]
        public string BusinessLocation { get; set; }
        [Column("nvarchar(100)")]
        public string State { get; set; }
        [Column("nvarchar(100)")]
        public string Street1 { get; set; }
        [Column("nvarchar(100)")]
        public string Street2 { get; set; }
        [Column("nvarchar(100)")]
        public string City { get; set; }
        [Column("nvarchar(100)")]
        public string Zip { get; set; }
        [Column("nvarchar(100)")]
        public string Country { get; set; }
        [Column("nvarchar(100)")]
        public string Currency { get; set; }
        [Column("nvarchar(100)")]
        public string Language { get; set; }
        [Column("nvarchar(100)")]
        public string Timezone { get; set; }
        [Column("nvarchar(100)")]
        public string TaxRegistrationNumber { get; set; }

        public DateTime? VatRegisterOn { get; set; }

    }
}
