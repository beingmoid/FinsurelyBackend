using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
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
