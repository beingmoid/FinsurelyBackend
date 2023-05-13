using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PanoramaBackend.Data.Entities;

namespace PanoramaBackend.Data.Repository
{
   
    public class PayrollRepository : EFRepository<Payroll, int>, IPayrollRepository
    {
        public PayrollRepository(AMFContext requestScope) : base(requestScope)
        {

        }


    }
    public interface IPayrollRepository : IEFRepository<Payroll, int>
    {

    }
}
