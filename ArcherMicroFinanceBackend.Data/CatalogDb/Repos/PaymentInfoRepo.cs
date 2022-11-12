


using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramBackend.Data.CatalogDb;

namespace PanoramBackend.Data.CatalogDb.Repos
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
