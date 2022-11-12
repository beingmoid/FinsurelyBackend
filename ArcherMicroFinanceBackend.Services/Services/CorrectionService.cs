using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class CorrectionService : BaseService<Corrections, int>, ICorrectionService
    {
        public CorrectionService(RequestScope scopeContext, ICorrectionRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface ICorrectionService : IBaseService<Corrections, int>
    {

    }
}
