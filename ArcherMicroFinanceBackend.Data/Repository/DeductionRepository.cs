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
    public class DeductionRepository : EFRepository<Deduction, int>, IDeductionRepository
    {
        public DeductionRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IDeductionRepository : IEFRepository<Deduction, int>
    {

    }

}
