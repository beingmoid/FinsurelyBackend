
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
    public class PayrollService : BaseService<Payroll, int>, IPayrollService
    {
        public PayrollService(RequestScope scopeContext, IPayrollRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IPayrollService : IBaseService<Payroll, int>
    {

    }
}
