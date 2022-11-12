using Microsoft.AspNetCore.Identity;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Data.DTOs
{
    public class ExtendedRole:ApplicationRole
    {
        public string Name { get; set; }
    }
}
