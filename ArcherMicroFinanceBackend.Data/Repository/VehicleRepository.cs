using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class VehicleRepository : EFRepository<Vehicle, int>, IVehicleRepository
    {
        public VehicleRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IVehicleRepository : IEFRepository<Vehicle, int>
    {

    }
}
