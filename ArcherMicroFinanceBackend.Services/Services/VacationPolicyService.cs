using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace PanoramBackend.Services.Services
{
    public class VacationPolicyService : BaseService<VacationPolicy, int>, IVacationPolicyService
    {
        public VacationPolicyService(RequestScope scopeContext, IVacationPolicyRepository repo) : base(scopeContext, repo)
        {

        }
        protected async override Task WhileInserting(IEnumerable<VacationPolicy> entities)
        {
            var entity = entities.SingleOrDefault();

            var nextPolicy = (await this.Get(x => x.EmploymentDetailId == entity.EmploymentDetailId && x.Applicable)).SingleOrDefault();
            if (nextPolicy != null)
            {
                nextPolicy.Applicable = false;
              await this.Update(nextPolicy.Id, nextPolicy);
            }
            entity.Applicable = true;


        }
    }
    public interface IVacationPolicyService : IBaseService<VacationPolicy, int>
    {

    }
}
