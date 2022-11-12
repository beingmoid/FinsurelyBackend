using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
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
