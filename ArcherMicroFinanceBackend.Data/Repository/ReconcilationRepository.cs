using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class ReconcilationRepository : EFRepository<Reconcilation, int>, IReconcilationRepository
    { 
    
        public ReconcilationRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface IReconcilationRepository : IEFRepository<Reconcilation, int>
    {

    }
}
