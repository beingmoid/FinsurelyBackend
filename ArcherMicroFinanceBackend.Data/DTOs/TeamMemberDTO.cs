using PanoramBackend.Services.Data.DTOs;
using PanoramBackend.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Data.DTOs
{
        public class TeamMemberDTO : BaseDTO
        {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UserDetails UserDetails { get; set; }
        public string Role { get; set; }
    }
}
