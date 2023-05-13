using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class AddressRepository : EFRepository<Address, int>, IAddressRepository
    {
        public AddressRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IAddressRepository : IEFRepository<Address, int>
    {

    }
}
