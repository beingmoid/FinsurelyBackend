using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PanoramaBackend.Data.Repository
{
    public class ComissionRateReposiotory : EFRepository<ComissionRate, int>, IComissionRateReposiotory
    {
        public ComissionRateReposiotory(AMFContext requestScope) : base(requestScope)
        {

        }
        protected override IQueryable<ComissionRate> Query => base.Query.Where(x=>x.IsActive);
    }
    public interface IComissionRateReposiotory : IEFRepository<ComissionRate, int>
    {

    }
}
