using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class EmploymentDetailService : BaseService<EmploymentDetails, int>, IEmploymentDetailService
    {
        public EmploymentDetailService(RequestScope scopeContext, IEmployeeDetailsRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IEmploymentDetailService : IBaseService<EmploymentDetails, int>
    {

    }
}
