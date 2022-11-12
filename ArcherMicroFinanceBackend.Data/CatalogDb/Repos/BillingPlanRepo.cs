

using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramBackend.Data.CatalogDb;

namespace PanoramBackend.Data.CatalogDb.Repos
{
    public class BillingPlanRepo : EFRepository<BillingPlan, int>, IBillingPlanRepo
    {
        public BillingPlanRepo(CatalogDbContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IBillingPlanRepo : IEFRepository<BillingPlan, int>
    {

    }
}
