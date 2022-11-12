using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PanoramBackend.Services.Services
{
    public class ComissionRateService : BaseService<ComissionRate, int>, IComissionRateService
    {
        private readonly IComissionRateReposiotory _repo;

        public ComissionRateService(RequestScope scopeContext, IComissionRateReposiotory repo) : base(scopeContext, repo)
        {
            _repo = repo;
        }
        protected async override Task WhileInserting(IEnumerable<ComissionRate> entities)
        {
            
            foreach (var item in entities)    
            {
                if (item.IsTpl)
                {
                    var elements = await _repo.Get(x => x.UserDetailId == item.UserDetailId && x.IsTpl);
                    foreach (var tpl in elements)
                    {
                        tpl.IsActive = false;
                        await _repo.Update(tpl.Id, tpl);
                        await _repo.SaveChanges();
                    }
                }
                else
                {
                    var elements = await _repo.Get(x => x.UserDetailId == item.UserDetailId && x.IsNonTpl);
                    foreach (var tpl in elements)
                    {
                        tpl.IsActive = false;
                        await _repo.Update(tpl.Id, tpl);
                        await _repo.SaveChanges();
                    }
                }
              

                item.IsActive=true;
            }


            //return await Task.FromResult( base.WhileInserting(entities));
        }
    }
    public interface IComissionRateService : IBaseService<ComissionRate, int>
    {

    }
}
