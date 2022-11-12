
using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
