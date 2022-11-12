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
    public class EmployeeFilesRepository : EFRepository<EmployeeFiles, int>, IEmployeeFilesRepository
    {
        public EmployeeFilesRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IEmployeeFilesRepository : IEFRepository<EmployeeFiles, int>
    {

    }

}
