
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class StaffOnBoardingService : BaseService<StaffOffBoarding, int>, IStaffOnBoardingService
    {
        public StaffOnBoardingService(RequestScope scopeContext, IStaffOffBoradingRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IStaffOnBoardingService : IBaseService<StaffOffBoarding, int>
    {

    }
}
