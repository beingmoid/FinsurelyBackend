using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramaBackend.Data.Repository
{
    public class PolicyTypeRepository : EFRepository<PolicyType, int>, IPolicyTypeRepository
    {
        public PolicyTypeRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IPolicyTypeRepository : IEFRepository<PolicyType, int>
    {

    }

}
