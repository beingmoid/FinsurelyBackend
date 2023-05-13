

using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.CatalogDb;

namespace PanoramaBackend.Data.CatalogDb.Repos
{
    public class PaymentHistoryRepo : EFRepository<PaymentHistory, int>, IPaymentHistoryRepo
    {
        public PaymentHistoryRepo(CatalogDbContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IPaymentHistoryRepo : IEFRepository<PaymentHistory, int>
    {

    }
}
