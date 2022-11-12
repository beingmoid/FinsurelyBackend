using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
