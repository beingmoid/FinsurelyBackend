using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramBackend.Data.Repository
{
    public class ServiceRepository : EFRepository<Service, int>, IServiceRepository
    {
        public ServiceRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IServiceRepository : IEFRepository<Service, int>
    {

    }

}
