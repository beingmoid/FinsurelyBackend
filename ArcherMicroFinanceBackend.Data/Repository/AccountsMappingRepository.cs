

using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
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
