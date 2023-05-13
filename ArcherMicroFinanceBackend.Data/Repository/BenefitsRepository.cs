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
