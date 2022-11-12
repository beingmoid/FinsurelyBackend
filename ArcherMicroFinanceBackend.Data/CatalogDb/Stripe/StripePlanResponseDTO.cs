using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.CatalogDb.Stripe
{
    public class StripePlanResponseDTO
    {
        public string StripeProductId { get; set; }
        public string StripePlanId { get; set; }
    }
}
