


using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.CatalogDb;

namespace PanoramaBackend.Data.CatalogDb.Repos
{
    public class PaymentInfoRepo : EFRepository<PaymentInfo, int>, IPaymentInfoRepo
    {
        public PaymentInfoRepo(CatalogDbContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IPaymentInfoRepo : IEFRepository<PaymentInfo, int>
    {

    }
}
