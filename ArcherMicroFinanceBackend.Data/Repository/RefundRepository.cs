using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class RefundRepository : EFRepository<Refund, int>, IRefundRepository
    {
        public RefundRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IRefundRepository : IEFRepository<Refund, int>
    {

    }
}
