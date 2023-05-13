using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class AttachmentRepository : EFRepository<Attachments, int>, IAttachmentRepository
    {
        public AttachmentRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IAttachmentRepository : IEFRepository<Attachments, int>
    {

    }
}
