using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.CatalogDb.Stripe
{
    public class CreateSubscriptionDTO
    {
        public string StripeSecretKey { get; set; }
        public string CustomerId { get; set; }
        public string PlanId { get; set; }
        public int Quantity { get; set; }
    }
}