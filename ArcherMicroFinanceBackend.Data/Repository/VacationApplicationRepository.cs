using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.Entities;

namespace PanoramBackend.Data.Repository
{
    public class VacationApplicationRepository : EFRepository<VacationApplication, int>, IVacationApplicationRepository
    {
        public VacationApplicationRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IVacationApplicationRepository : IEFRepository<VacationApplication, int>
    {

    }
}
