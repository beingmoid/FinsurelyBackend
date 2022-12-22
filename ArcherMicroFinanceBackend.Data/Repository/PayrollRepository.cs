using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PanoramaBackend.Data.Entities;

namespace PanoramBackend.Data.Repository
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
