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
    public class ExpenseCategoryService : BaseService<ExpenseCategory, int>, IExpenseCategoryService
    {
        public ExpenseCategoryService(RequestScope scopeContext, IExpenseCategoryReposiotory repo) : base(scopeContext, repo)
        {

        }
        
    }
    public interface IExpenseCategoryService : IBaseService<ExpenseCategory, int>
    {

    }
}
