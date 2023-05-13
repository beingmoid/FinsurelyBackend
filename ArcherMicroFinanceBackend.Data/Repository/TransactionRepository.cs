using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Repository
{
    public class TransactionRepository : EFRepository<Transaction, int>, ITransactionRepository
    {
        public TransactionRepository(AMFContext requestScope) : base(requestScope)
        {

        }
    }
    public interface ITransactionRepository : IEFRepository<Transaction, int>
    {

    }
}
