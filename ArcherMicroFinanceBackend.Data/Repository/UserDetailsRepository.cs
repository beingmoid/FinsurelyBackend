using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class UserDetailsRepository : EFRepository<UserDetails, int>, IUserDetailsRepository
    {
        public UserDetailsRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IUserDetailsRepository : IEFRepository<UserDetails, int>
    {

    }
}
