
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class TeamsService : BaseService<Teams, int>, ITeamsService
    {
        public TeamsService(RequestScope scopeContext, ITeamsRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface ITeamsService : IBaseService<Teams, int>
    {

    }
}
