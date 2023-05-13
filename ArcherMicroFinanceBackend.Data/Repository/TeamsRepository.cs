using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramaBackend.Data.Repository
{
    public class TeamsRepository : EFRepository<Teams, int>, ITeamsRepository
    {
        public TeamsRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface ITeamsRepository : IEFRepository<Teams, int>
    {

    }

}
