using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.Entities;

namespace PanoramaBackend.Services.Services
{
    public class VacationApplicationService : BaseService<VacationApplication, int>, IVacationApplicationService
    {
        public VacationApplicationService(RequestScope scopeContext, IVacationApplicationRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IVacationApplicationService : IBaseService<VacationApplication, int>
    {

    }
}
