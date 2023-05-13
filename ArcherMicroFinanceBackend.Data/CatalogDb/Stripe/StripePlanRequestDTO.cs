using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.CatalogDb.Stripe
{
    public class StripePlanRequestDTO
    {
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Interval { get; set; }
        public string StripeSecretKey { get; set; }
        public bool IsRecurring { get; set; }

        public string StripeProductId { get; set; }
        public string StripePlanId { get; set; }
    }
}
