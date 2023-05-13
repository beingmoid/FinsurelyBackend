using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class AddressService : BaseService<Address, int>, IAddressService
    {
        public AddressService(RequestScope scopeContext, IAddressRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IAddressService : IBaseService<Address, int>
    {

    }
}
