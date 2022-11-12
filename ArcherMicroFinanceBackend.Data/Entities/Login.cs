using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Entities
{
    public class Login : BaseEntity<int> 
    {
   
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string UserId { get; set; }
   
    }
}
