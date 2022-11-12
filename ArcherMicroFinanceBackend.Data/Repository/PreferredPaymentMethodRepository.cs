using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class PreferredPaymentMethodRepository : EFRepository<PreferredPaymentMethod, int>, IPreferredPaymentMethodRepository
    {
        public PreferredPaymentMethodRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IPreferredPaymentMethodRepository : IEFRepository<PreferredPaymentMethod, int>
    {

    }
}
