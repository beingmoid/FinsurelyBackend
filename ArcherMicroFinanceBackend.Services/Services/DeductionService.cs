using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
