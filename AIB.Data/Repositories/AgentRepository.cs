using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIB.Data.Repositories
{
    public class AgentRepository : EFRepository<Agent,int>, IAgentRepository
    {
        public AgentRepository(AIBContext requestScope):base(requestScope)
        {

        }
    }
    public interface IAgentRepository : IEFRepository<Agent, int>
    {

    }
}
