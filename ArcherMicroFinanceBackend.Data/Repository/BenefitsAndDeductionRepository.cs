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
    public class BenefitsAndDeductionRepository : EFRepository<BenefitsAndDeduction, int>, IBenefitsAndDeductionRepository
    {
        public BenefitsAndDeductionRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IBenefitsAndDeductionRepository :IEFRepository<BenefitsAndDeduction, int>
    {

    }

}
