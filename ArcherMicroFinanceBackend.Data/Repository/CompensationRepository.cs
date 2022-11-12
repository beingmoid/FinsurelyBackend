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
