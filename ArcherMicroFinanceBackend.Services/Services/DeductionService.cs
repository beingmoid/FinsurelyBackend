using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class DeductionService : BaseService<Deduction, int>, IDeductionService
    {
        public DeductionService(RequestScope scopeContext, IDeductionRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IDeductionService : IBaseService<Deduction, int>
    {

    }
}
