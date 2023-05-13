using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Repositories
{
    public class VehicleModelRepository : EFRepository<VehicleModel,int>, IVehicleModelRepository
    {
        public VehicleModelRepository(AIBContext requestScope):base(requestScope)
        {

        }
    }
    public interface IVehicleModelRepository : IEFRepository<VehicleModel, int>
    {

    }
}
