using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
{
    public class CorrectionRepository : EFRepository<Corrections, int>, ICorrectionRepository
    { 
    
        public CorrectionRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface ICorrectionRepository : IEFRepository<Corrections, int>
    {

    }
}
