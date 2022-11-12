using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
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
