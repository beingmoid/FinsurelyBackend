using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static PanoramaBackend.Data.CatalogDb.Enums;

namespace PanoramaBackend.Data.CatalogDb
{
    [Table("CatalogDb_SubscriptionPlan")]
    public class SubscriptionPlans : BaseEntity<int>
    {


        [Required]
        [Column("cls_SubscriptionPlanKey", TypeName = "varchar(50)")]
        public string SubscriptionPlanKey { get; set; }

        [Required]
        [Column("cls_Currency", TypeName = "varchar(5)")]
        public string Currency { get; set; }

        [Required]
        [Column("cls_Name", TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Required]
        [Column("cls_PricePerUser", TypeName = "decimal(12,4)")]
        public decimal PricePerUser { get; set; }

        [Required]
        [Column("cls_Description", TypeName = "varchar(MAX)")]
        public string Description { get; set; }

        [Column("cls_Storage")]
        public double Storage { get; set; }

        [Required]
        [Column("cls_FreeUsersAllowed")]
        public int FreeUsersAllowed { get; set; }

        [Column("cls_IsEnabled")]
        public bool IsEnabled { get; set; }

        [Column("cls_IsRecurring")]
        public bool IsRecurring { get; set; }

        [Required]
        [Column("cls_clb_BillingPlanId")]
        public int BillingPlanId { get; set; }

        [Column("cls_FreeDays")]
        public int? FreeDays { get; set; }

        [Required]
        [Column("cls_SignUpURL", TypeName = "varchar(500)")]
        public string SignUpURL { get; set; }

        [Required]
        [Column("cls_StripePlanId", TypeName = "varchar(100)")]
        public string StripePlanId { get; set; }

        [Required]
        [Column("cls_StripeProductId", TypeName = "varchar(100)")]
        public string StripeProductId { get; set; }

        [ForeignKey("BillingPlanId")]
        public virtual BillingPlan BillingPlan { get; set; }
    }
    [Table("Casolve_License_Order")]
    public class Order : BaseEntity<int>
    { 
        [Column("clo_clc_CompanyId")]
        public byte[] CompanyId { get; set; }

        [Column("clo_CompanyGUID")]
        public byte[] CompanyGUID { get; set; }

        [Column("clo_cls_SubscriptionPlanId")]
        public int? SubscriptionPlanId { get; set; }

        [Column("clo_Payment", TypeName = "decimal(12,4)")]
        public decimal Payment { get; set; }

        [Column("clo_NumberOfUsers")]
        public int NumberOfUsers { get; set; }

        [Column("clo_IsFree")]
        public bool IsFree { get; set; }

        [Column("clo_EndDate")]
        public DateTime? EndDate { get; set; }

        [Column("clo_RecurringDate")]
        public DateTime? RecurringDate { get; set; }

        [Column("clo_StripeSubscriptionId")]
        public string StripeSubscriptionId { get; set; }

        [Column("clo_SubscriptionStatus")]
        public SubscriptionStatus SubscriptionStatus { get; set; }

        [Column("clo_SubscriptionMessage")]
        public string SubscriptionMessage { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("SubscriptionPlanId")]
        public virtual SubscriptionPlans SubscriptionPlan { get; set; }
    }
    [Table("CatalogDb_ErrorLog")]
    public class ErrorLog:BaseEntity<int>
    {

        public DateTime CreatedDate { get; set; }

        [Column("cle_StackTrace")]
        public string StackTrace { get; set; }

        [Column("cle_ExceptionMessage")]
        public string ExceptionMessage { get; set; }

        [Column("cle_Source")]
        public string Source { get; set; }
    }
}
