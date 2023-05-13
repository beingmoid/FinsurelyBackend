using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Repositories
{
    public class BrokerRepository : EFRepository<Broker,int>, IBrokerRepository
    {
        public BrokerRepository(AIBContext requestScope):base(requestScope)
        {

        }
    }
    public interface IBrokerRepository : IEFRepository<Broker, int>
    {

    }
}
