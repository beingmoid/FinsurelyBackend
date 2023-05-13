using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class CorrectionService : BaseService<CompanyInformation, int>, ICorrectionService
    {
        public CorrectionService(RequestScope scopeContext, ICorrectionRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface ICorrectionService : IBaseService<CompanyInformation, int>
    {

    }
}
