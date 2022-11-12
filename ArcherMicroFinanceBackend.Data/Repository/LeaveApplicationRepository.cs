using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramBackend.Data.Repository
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
