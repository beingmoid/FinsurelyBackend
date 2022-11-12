using NukesLab.Core.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramBackend.Data.Entities;

namespace PanoramBackend.Services.Data.DTOs
{
    public class ExtendedUser: ApplicationUser
    {

        public UserDetails UserDetails { get; set; }

    }
    public enum MarriageStatus
    {
        Single=1,
        Married,
        Divorced
    }
    public enum TypeOfUsers
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
