
using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
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
