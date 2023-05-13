using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PanoramaBackend.Data.Entities;

namespace PanoramaBackend.Services.Services
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
