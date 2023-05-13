using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Repositories
{
    public class MotorTypeRepository : EFRepository<MotorType,int>, IMotorTypeRepository
    {
        public MotorTypeRepository(AIBContext requestScope):base(requestScope)
        {

        }
    }
    public interface IMotorTypeRepository : IEFRepository<MotorType, int>
    {

    }
}
