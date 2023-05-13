using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
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
