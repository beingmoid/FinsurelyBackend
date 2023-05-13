using AIB.Data.Entities;
using AIB.Data.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIB.Data.Repositories
{
    public class ExpenseRepository : EFRepository<Expense,int>, IExpenseRepository
    {
        public ExpenseRepository(AIBContext requestScope):base(requestScope)
        {

        }
        public override IQueryable<Expense> Query => base.Query.Include(x => x.ParentExpense);
    }
    public interface IExpenseRepository : IEFRepository<Expense, int>
    {

    }
}
