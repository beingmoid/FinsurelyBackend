using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
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
