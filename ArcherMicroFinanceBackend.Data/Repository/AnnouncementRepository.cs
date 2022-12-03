using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.Entities;

namespace PanoramBackend.Data.Repository
{
    public class AnnouncementRepository : EFRepository<Announcement, int>, IAnnouncementRepository
    {
        public AnnouncementRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IAnnouncementRepository : IEFRepository<Announcement, int>
    {

    }
}
