using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Services.Services
{
    public class EmployeeFilesService : BaseService<EmployeeFiles, int>, IEmployeeFilesService
    {
        public EmployeeFilesService(RequestScope scopeContext, IEmployeeFilesRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IEmployeeFilesService : IBaseService<EmployeeFiles, int>
    {

    }
}
