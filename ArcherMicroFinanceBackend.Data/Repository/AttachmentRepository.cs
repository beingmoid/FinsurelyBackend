using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
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
