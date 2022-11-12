

using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class AccountsMappingRepository : EFRepository<AccountsMapping, int>, IAccountsMappingRepository
    {
        public AccountsMappingRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IAccountsMappingRepository : IEFRepository<AccountsMapping, int>
    {

    }
}
