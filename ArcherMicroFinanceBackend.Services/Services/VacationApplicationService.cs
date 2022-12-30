using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.Entities;

namespace PanoramBackend.Services.Services
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
