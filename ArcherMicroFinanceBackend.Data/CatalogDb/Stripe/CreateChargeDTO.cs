using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.CatalogDb.Stripe
{
    public class CreateChargeDTO
    {
        public string StripeSecretKey { get; set; }
        public string CustomerId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
    }
}