using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using static NukesLab.Core.Common.Constants;
using PanoramaBackend.Data.Entities;

namespace PanoramBackend.Services.Services
{
    public class AnnoucementService : BaseService<Announcement, int>, IAnnoucementService
    {
        public AnnoucementService(RequestScope scopeContext, IAnnouncementRepository repo) : base(scopeContext, repo)
        {

        }

    }
    public interface IAnnoucementService : IBaseService<Announcement, int>
    {
      
    }
}
