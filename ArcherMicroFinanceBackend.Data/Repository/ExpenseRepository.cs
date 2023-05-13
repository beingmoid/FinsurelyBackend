using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PanoramaBackend.Data.Entities;

namespace PanoramaBackend.Data.Repository
{
    public class ExpenseRepository : EFRepository<Expense, int>, IExpenseRepository
    {
        public ExpenseRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IExpenseRepository : IEFRepository<Expense, int>
    {

    }

}
