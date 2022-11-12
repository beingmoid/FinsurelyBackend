using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class AttendanceRepository : EFRepository<Attendance, int>, IAttendanceRepository

    { 
    
        public AttendanceRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IAttendanceRepository : IEFRepository<Attendance, int>
    {

    }
}
