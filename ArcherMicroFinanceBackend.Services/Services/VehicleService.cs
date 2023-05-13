using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class VehicleService : BaseService<Vehicle, int>, IVehicleService
    {
        public VehicleService(RequestScope scopeContext, IVehicleRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IVehicleService : IBaseService<Vehicle, int>
    {

    }
}
