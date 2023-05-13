using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class DocumentRepository : EFRepository<Documents, int>, IDocumentRepository
    { 
    
        public DocumentRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IDocumentRepository : IEFRepository<Documents, int>
    {

    }
}
