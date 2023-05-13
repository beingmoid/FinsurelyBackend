using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramaBackend.Data.Repository
{
    public class LeaveApplicationRepository : EFRepository<LeaveApplication, int>, ILeaveApplicationRepository
    {
        public LeaveApplicationRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface ILeaveApplicationRepository : IEFRepository<LeaveApplication, int>
    {

    }

}
