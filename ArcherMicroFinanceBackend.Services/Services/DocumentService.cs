using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
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
