using AIB.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Entities
{
    public class ExtendedUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public string DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        public bool isManager { get; set; }
        public Guid? BranchId { get; set; }
        public Branch Branch { get; set; }
        public bool isAgent { get; set; }
        public MarriageStatus MarriageStatus { get; set; }
        public TypeOfUser TypeOfUser { get; set; }
        public Gender Gender { get; set; }


       





    }
    public enum MarriageStatus
    {
        Single=1,
        Married,
        Divorced
    }
    public enum TypeOfUser
    {
        Agent=1,
        Broker
    }
    public enum Gender
    {
        Male=1,
        Female
    }
}
