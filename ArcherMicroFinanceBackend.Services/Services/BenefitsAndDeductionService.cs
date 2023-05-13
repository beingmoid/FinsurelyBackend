using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace PanoramaBackend.Services.Services
{
    public class BenefitsAndDeductionService : BaseService<BenefitsAndDeduction, int>, IBenefitsAndDeductionService
    {
        public BenefitsAndDeductionService(RequestScope scopeContext, IBenefitsAndDeductionRepository repo) : base(scopeContext, repo)
        {

        }
        protected override Task WhileInserting(IEnumerable<BenefitsAndDeduction> entities)
        {
            
            var entity = entities.SingleOrDefault();

            if (entity.Benefits.Count==0 && entity.Deduction.Count==0)
            {
                throw new ServiceException("Benefit and Deduction both can not be empty");
            }
            entity.Applied = false;
            return base.WhileInserting(entities);
        }
    }
    public interface IBenefitsAndDeductionService : IBaseService<BenefitsAndDeduction, int>
    {

    }
}
