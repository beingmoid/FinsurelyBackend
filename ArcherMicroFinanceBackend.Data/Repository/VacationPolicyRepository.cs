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
    public class VacationPolicyRepository : EFRepository<VacationPolicy, int>, IVacationPolicyRepository
    {
        public VacationPolicyRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IVacationPolicyRepository : IEFRepository<VacationPolicy, int>
    {

    }

}
