using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
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
