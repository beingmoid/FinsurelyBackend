
using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
