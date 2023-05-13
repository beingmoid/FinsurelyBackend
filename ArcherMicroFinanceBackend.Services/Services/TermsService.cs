using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class TermsService : BaseService<Terms, int>, ITermsService
    {
        public TermsService(RequestScope scopeContext, ITermsRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface ITermsService : IBaseService<Terms, int>
    {

    }
}
