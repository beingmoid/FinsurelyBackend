using PanoramaBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PanoramaBackend.Data.Repository
{
    public class UserCompanyInformationRepository : EFRepository<UserCompanyInformation, int>, IUserCompanyInformationRepository
    {
        public UserCompanyInformationRepository(AMFContext requestScope) : base(requestScope)
        {

        }
  

    }

    public interface IUserCompanyInformationRepository : IEFRepository<UserCompanyInformation, int>
    {

    }
}
