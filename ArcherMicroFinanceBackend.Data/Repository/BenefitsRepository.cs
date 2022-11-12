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
    public class BenefitsRepository : EFRepository<Benefits, int>, IBenefitsRepository
    {
        public BenefitsRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IBenefitsRepository : IEFRepository<Benefits, int>
    {

    }

}
