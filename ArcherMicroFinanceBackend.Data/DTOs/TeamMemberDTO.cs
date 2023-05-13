using PanoramaBackend.Services.Data.DTOs;
using PanoramaBackend.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Data.DTOs
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
