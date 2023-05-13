using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class CorrectionRepository : EFRepository<CompanyInformation, int>, ICorrectionRepository
    { 
    
        public CorrectionRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface ICorrectionRepository : IEFRepository<CompanyInformation, int>
    {

    }
}
