using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using static NukesLab.Core.Common.Constants;

namespace PanoramBackend.Services.Services
{
    public class AccountDetailTypeService : BaseService<AccountDetailType, int>, IAccountDetailTypeService
    {
        public AccountDetailTypeService(RequestScope scopeContext, IAccountsDetailTypeRepository repo) : base(scopeContext, repo)
        {

        }
        public async Task<List<AccountDetailType>> GetDetails(int accountTypeId) {

            var list = await this.Get(x => x.AccountTypeId == accountTypeId);
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            return list.ToList();
                
                }
    }
    public interface IAccountDetailTypeService : IBaseService<AccountDetailType, int>
    {
       Task<List<AccountDetailType>> GetDetails(int accountTypeId);
    }
}
