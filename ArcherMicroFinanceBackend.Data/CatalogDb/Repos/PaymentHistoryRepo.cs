

using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramBackend.Data.CatalogDb;

namespace PanoramBackend.Data.CatalogDb.Repos
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
