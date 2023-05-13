using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class TermsRepository : EFRepository<Terms, int>, ITermsRepository
    {
        public TermsRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface ITermsRepository : IEFRepository<Terms, int>
    {

    }
}
