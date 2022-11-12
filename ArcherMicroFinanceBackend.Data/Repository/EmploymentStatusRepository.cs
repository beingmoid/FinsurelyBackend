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
    public class EmploymentStatusRepository : EFRepository<EmploymentStatus, int>, IEmploymentStatusRepository
    {
        public EmploymentStatusRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IEmploymentStatusRepository : IEFRepository<EmploymentStatus, int>
    {

    }

}
