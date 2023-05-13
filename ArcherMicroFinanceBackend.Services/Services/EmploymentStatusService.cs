
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class EmploymentStatusService : BaseService<EmploymentStatus, int>, IEmploymentStatusService
    {
        public EmploymentStatusService(RequestScope scopeContext, IEmploymentStatusRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IEmploymentStatusService : IBaseService<EmploymentStatus, int>
    {

    }
}
