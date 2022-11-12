using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramBackend.Data.CatalogDb
{
    [Table("CatalogDb_License_Company")]
    public class Company : BaseEntity<int>
    {

        [Column("clc_Name", TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column("clc_Email", TypeName = "varchar(100)")]
        public string Email { get; set; }   
        public bool EmailVerified { get; set; }

        [Column("clc_Contact", TypeName = "varchar(50)")]
        public string Contact { get; set; }

        [Column("clc_CompanyName", TypeName = "varchar(100)")]
        public string CompanyName { get; set; }

        [Column("clc_CompanyURL", TypeName = "varchar(200)")]
        public string CompanyURL { get; set; }

        [Column("clc_BlobImageURI", TypeName = "varchar(500)")]
        public string BlobImageURI { get; set; }

        [Column("clc_ImageName", TypeName = "varchar(50)")]
        public string ImageName { get; set; }
        public byte[] CompanyGUID { get; set; }

        [Column("clc_cls_SubscriptionPlanId")]
        public int? SubscriptionPlanId { get; set; }

        [Column("clc_IsEnabled")]
        public bool IsEnabled { get; set; }

        [ForeignKey("SubscriptionPlanId")]
        public virtual SubscriptionPlans SubscriptionPlan { get; set; }
    }
    public class CardInfo : BaseEntity<int>
    {

        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int PostalCode { get; set; }
        public int SubscriptionPlanId { get; set; }

        [ForeignKey("SubscriptionPlanId")]
        public virtual SubscriptionPlans SubscriptionPlan { get; set; }
    }
}