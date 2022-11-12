using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Data.DTOs
{
    public class ClaimTypeDTO
    {
        public string ClaimTypeName { get; set; }
        public ClaimValueDTO ClaimValue { get; set; }
    }
}
