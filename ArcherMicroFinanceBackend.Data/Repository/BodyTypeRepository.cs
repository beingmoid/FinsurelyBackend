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
    public class BodyTypeRepository : EFRepository<BodyType, int>, IBodyTypeRepository
    {
        public BodyTypeRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IBodyTypeRepository : IEFRepository<BodyType, int>
    {

    }

}
