using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramBackend.Data.Repository
{
    public class BankDetailRepository : EFRepository<BankDetails, int>, IBankDetailRepository
    {
        public BankDetailRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IBankDetailRepository : IEFRepository<BankDetails, int>
    {

    }

}
