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
