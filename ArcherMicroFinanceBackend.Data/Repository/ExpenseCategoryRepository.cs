using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PanoramaBackend.Data.Entities;

namespace PanoramBackend.Data.Repository
{
    public class ExpenseCategoryRepository : EFRepository<ExpenseCategory, int>, IExpenseCategoryReposiotory
    {
        public ExpenseCategoryRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IExpenseCategoryReposiotory : IEFRepository<ExpenseCategory, int>
    {

    }

}
