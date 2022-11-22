using Microsoft.AspNetCore.Http;
using NukesLab.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NukesLab.Core.Repository
{
    public interface IUserProvider
    {
        string UserId { get; }
    }
    public class IdentityUserProvider : IUserProvider
    {
        public string UserId { get; private set; }

        public IdentityUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated ?? false)
            {
                UserId = httpContextAccessor.HttpContext.User.GetUserId();
            }
        }
    }
}
