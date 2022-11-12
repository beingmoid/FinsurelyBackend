using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class UserDetailsService: BaseService<UserDetails, int>,IUserDetailsService
    {
        public UserDetailsService(RequestScope scopeContext, IUserDetailsRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IUserDetailsService : IBaseService<UserDetails, int>
    {

    }
}
