using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class DocumentService : BaseService<Documents, int>, IDocumentService
    {
        public DocumentService(RequestScope scopeContext, IDocumentRepository repo) : base(scopeContext, repo)
        {

        }
    }
    public interface IDocumentService : IBaseService<Documents, int>
    {

    }
}
