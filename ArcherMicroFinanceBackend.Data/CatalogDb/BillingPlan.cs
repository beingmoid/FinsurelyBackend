using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PanoramaBackend.Data.CatalogDb
{
    [Table("CatalogDb_BillingPlan")]
    public class BillingPlan : BaseEntity<int>
    {
   

        [Required]
        [Column("clb_Name", TypeName = "varchar(50)")]
        public string Name { get; set; }
    }
    public class PaymentHistory : BaseEntity<int>
    {
        
        public byte[] tenantId { get; set; }
        public int? SubscriptionPlanId { get; set; }

        [Column(TypeName = "decimal(12,4)")]
        public decimal Payment { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RecurringDate { get; set; }
        public string InvoiceURL { get; set; }
        public string StripeRecieptURL { get; set; }
        public string Currency { get; set; }
        public int NumberOfUsers { get; set; }


        [ForeignKey("SubscriptionPlanId")]
        public virtual SubscriptionPlans SubscriptionPlan { get; set; }
    }

    public class PaymentInfo : BaseEntity<int>
    {

        public DateTime NextPayment { get; set; }
        public double Price { get; set; }
        //public int SubscriptionPlanId { get; set; }
        public int CompanyId { get; set; }

        //[ForeignKey("SubscriptionPlanId")]
        //public virtual SubscriptionPlans SubscriptionPlan { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
    [Table("CatalogDb_StripeConfiguration")]
    public class StripeConfigurations : BaseEntity<int>
    {


        [Required]
        [Column("cls_StripePublishableKey", TypeName = "varchar(200)")]
        public string StripePublishableKey { get; set; }

        [Required]
        [Column("cls_StripeSecretKey", TypeName = "varchar(200)")]
        public string StripeSecretKey { get; set; }
 



    }
}
