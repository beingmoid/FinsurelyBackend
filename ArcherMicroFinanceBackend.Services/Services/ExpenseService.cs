using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PanoramaBackend.Data.Entities;

namespace PanoramBackend.Services.Services
{
    public class ExpenseService : BaseService<Expense, int>, IExpenseService
    {
        public ExpenseService(RequestScope scopeContext, IExpenseRepository repo) : base(scopeContext, repo)
        {

        }
   
    }
    public interface IExpenseService : IBaseService<Expense, int>
    {

    }
}
