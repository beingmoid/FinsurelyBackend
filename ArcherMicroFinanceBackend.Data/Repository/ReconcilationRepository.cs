using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramBackend.Data.Repository
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
