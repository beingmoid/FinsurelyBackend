using PanoramaBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PanoramaBackend.Data.Repository
{
    public class SetupClientRepository : EFRepository<SetupClient, int>, ISetupClientRepository
    {
        public SetupClientRepository(AMFContext requestScope) : base(requestScope)
        {

        }


    }

    public interface ISetupClientRepository : IEFRepository<SetupClient, int>
    {

    }
}
