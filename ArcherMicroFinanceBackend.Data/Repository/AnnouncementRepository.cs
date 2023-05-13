using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.Entities;

namespace PanoramaBackend.Data.Repository
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
