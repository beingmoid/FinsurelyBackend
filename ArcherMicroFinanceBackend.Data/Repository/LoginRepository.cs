using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class LoginRepository : EFRepository<Login, int>, ILoginRepository
    {
        public LoginRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface ILoginRepository : IEFRepository<Login, int>
    {

    }
}
