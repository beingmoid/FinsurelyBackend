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
    public class CompensationRepository : EFRepository<Compensation, int>, ICompensationRepository
    {
        public CompensationRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface ICompensationRepository : IEFRepository<Compensation, int>
    {

    }

}
