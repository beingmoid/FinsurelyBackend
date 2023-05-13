using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Services.Data.DTOs;
using PanoramaBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Api.Controllers
{

    public class UserDetailController : BaseController<UserDetails,int>
    {
        private readonly UserManager<ExtendedUser> _userManager;

        public UserDetailController(RequestScope requestScope,IUserDetailsService service, UserManager<ExtendedUser> userManager)
            :base(requestScope,service)
        {
            _userManager = userManager;
        }
        [HttpPost("CreateUser")]
        public async Task<BaseResponse> CreateUser([FromBody] TeamMemberDTO user)
        {
            ExtendedUser extendedUser = new ExtendedUser()
            {
                Email = user.Email
         };
            extendedUser.UserDetails.Add(user.UserDetails);
            var result = await _userManager.CreateAsync(extendedUser);
            if (result.Succeeded)
            {
                OtherConstants.isSuccessful = true;
                OtherConstants.messageType = MessageType.Success;
                return constructResponse(result);
            }
            else
            {
                OtherConstants.isSuccessful = true;
                OtherConstants.messageType = MessageType.Success;
                return constructResponse(result);
            }
        }
    }
}
