using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureIdentity.Context.Entities
{
    public class Company
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

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
    public record RegionalSetting
    {


    }
    public record BusinessRegistration
    {
      
    }
}
