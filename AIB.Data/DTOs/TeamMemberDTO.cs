using AIB.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.DTOs
{
        public class TeamMemberDTO : BaseDTO
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string Password { get; set; }
            public string LastName { get; set; }
            public string PrimaryPhone { get; set; }
            public string SecondaryPhone { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Nationality { get; set; }
            public string DateOfBirth { get; set; }
            public string JobTitle { get; set; }
            public string Role { get; set; }
            public bool isManager { get; set; }
            public bool isAgent { get; set; }
            public string BranchId { get; set; }
            public MarriageStatus MarriageStatus { get; set; }
            public TypeOfUser TypeOfUser { get; set; }
            public Gender Gender { get; set; }
        
    }
}
