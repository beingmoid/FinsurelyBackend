using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIB.Data.Repositories
{
    public class OutstandingsRepository : EFRepository<Outstandings,int>, IOutstandingsRepository
    {
        public OutstandingsRepository(AIBContext requestScope):base(requestScope)
        {

        }
 
    }
    public interface IOutstandingsRepository : IEFRepository<Outstandings, int>
    {

    }
}
