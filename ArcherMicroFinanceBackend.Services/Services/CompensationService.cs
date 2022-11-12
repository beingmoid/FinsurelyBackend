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
    public class CompensationService : BaseService<Compensation, int>, ICompensationService
    {
        public CompensationService(RequestScope scopeContext, ICompensationRepository repo) : base(scopeContext, repo)
        {

        }
        protected async override Task WhileInserting(IEnumerable<Compensation> entities)
        {
            var entity = entities.SingleOrDefault();
            var prevEntity = (await this.Get(x => x.EmploymentDetailId == entity.EmploymentDetailId && x.Effective)).SingleOrDefault();
            if(prevEntity!=null)
            {
                prevEntity.Effective = false;
                prevEntity.Expired = true;
                prevEntity.ExpiryDate = DateTime.Now;
                entity.Effective = true;
                await this.Update(prevEntity.Id, prevEntity);
            }
            else
            {
                entity.Effective = true;
            }   
         
        }
    }
    public interface ICompensationService : IBaseService<Compensation, int>
    {

    }
}
