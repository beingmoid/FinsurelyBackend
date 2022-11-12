using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
