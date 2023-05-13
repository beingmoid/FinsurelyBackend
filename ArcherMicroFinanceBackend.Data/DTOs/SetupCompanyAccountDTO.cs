using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.DTOs
{
    public class SetupCompanyAccountDTO
    {
        public Guid Id { get; set; }

        public string LegalStructure { get; set; }

        public string EmailAddress { get; set; }

        public string Website { get; set; }

        public string ContactInformation { get; set; }

        public string LogoUrl { get; set; }

        public string LogoBase64 { get; set; }

        public DateTime? PayrollDate { get; set; }

        public string VATRegistrationNumber { get; set; }

        public bool isMigrationRequired { get; set; }

        public string OrganizationName { get; set; }

        public string BusinessLocation { get; set; }

        public string State { get; set; }

        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public string Currency { get; set; }

        public string Language { get; set; }

        public string Timezone { get; set; }

        public string TaxRegistrationNumber { get; set; }

        public DateTime? VatRegisterOn { get; set; }

        public bool SignedUpWithMicrosoft { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string DisplayName { get; set; }

        public string Password { get; set; }

        public string ImageUrl { get; set; }

        public string LoginEmail { get; set; }
    }

}
