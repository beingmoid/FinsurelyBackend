using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;

namespace PanoramaBackend.Services.Services
{
    public class UserCompanyInformationService : BaseService<UserCompanyInformation, int>, IUserCompanyInformationService
    {
        public UserCompanyInformationService(RequestScope scopeContext, IUserCompanyInformationRepository repo) : base(scopeContext, repo)
        {

        }
    }

    public interface IUserCompanyInformationService : IBaseService<UserCompanyInformation, int>
    {

    }
}
