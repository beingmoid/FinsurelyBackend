using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.CatalogDb.Stripe
{
    public class CreateCustomerDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string StripeSecretKey { get; set; }
        public string StripePaymentToken { get; set; }
    }
}