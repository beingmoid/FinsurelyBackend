
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.DTOs;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Data.DTOs;
using PanoramaBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Controllers
{

    public class AuthController : BaseController
    {
        private readonly IAuthService _service;
        private readonly IExtendedUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly SignInManager<ExtendedUser> _signInManager;
        private readonly UserManager<ExtendedUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IUserCompanyInformationService _userCompanyInformationService;
        private readonly ISetupClientService _setupClientService;

        public AuthController(RequestScope scope, IExtendedUserRepository repo,
            IAuthService service,
            IMapper mapper,
            SignInManager<ExtendedUser> signInManager,
            UserManager<ExtendedUser> userManager,
            IUserCompanyInformationService userCompanyInformationService,
            ISetupClientService setupClientService,
            IWebHostEnvironment env) : base(scope, null)
        {
            _service = service;
            _repo = repo;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _env = env;
            _userCompanyInformationService = userCompanyInformationService;
            _setupClientService = setupClientService;

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<BaseResponse> ProcessLogin([FromBody] LoginInfo loginInfo) => constructResponse(await _service.ProcessLogin(loginInfo));

        [HttpGet("GenerateForgotPasswordToken")]
        public async Task<BaseResponse> GenerateForgotPasswordToken(string email) => constructResponse(await _service.GenerateForgotPasswordToken(email));

        [HttpPost("ResetPasswordWithToken")]
        public async Task<BaseResponse> ResetPasswordWithToken([FromBody] ResetPasswordDTO model) => constructResponse(await _service.ResetPasswordWithToken(model));

        [HttpPost("CreateNewPassword")]
        public async Task<BaseResponse> CreateNewPassword([FromBody] ResetPasswordDTO model) => constructResponse(await _service.CreateNewPassword(model));
        [HttpPost("CreateRoleWithClaims")]
        public async Task<BaseResponse> CreateRoleWithClaims([FromBody] RoleClaimsDTO model) => constructResponse(await _service.CreateRoleWithClaims(model));
        [HttpGet("Account")]
        public async Task<BaseResponse> GetUsers() => constructResponse(await _service.GetAccount());

        [HttpGet("GetAllRoles")]
        public async Task<BaseResponse> GetAllRoles() => constructResponse(await _service.GetAllRoles());
        [AllowAnonymous]
        [HttpPost("OnBoardingProcess")]
        public async Task<BaseResponse> OnBoardingProcess([FromBody] OnBoarding model) => constructResponse(await _repo.OnBoardingMember(model));
        [HttpGet]
        [Route("Logout")]
        public async Task<BaseResponse> LogoutAsync()
        {
            await _service.Logout();
            return constructResponse(true);
        }

        [HttpGet("StatementConfig")]
        public BaseResponse StatementConfigs()
        {
            var myJsonString = System.IO.File.ReadAllText("EmailTemplates\\statementConfig.json");
            var Jobject = JsonConvert.DeserializeObject<StatementConfig>(myJsonString);
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            return constructResponse(Jobject);
        }
        [HttpPost("UpdateStatementConfig")]
        public BaseResponse UpdateStatementConfig([FromBody] StatementConfig config)
        {
            var myJsonString = System.IO.File.ReadAllText("EmailTemplates\\statementConfig.json");
            var Jobject = JObject.FromObject(config);
            var path = _env.ContentRootPath + "\\EmailTemplates\\statementConfig.json";
            using (StreamWriter file = System.IO.File.CreateText(path))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                Jobject.WriteTo(writer);
            }
            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            return constructResponse(config);
        }

        [HttpPost("Setup")]
        public async Task<BaseResponse> SetupCompanyAccount([FromBody] SetupCompanyAccountDTO dto)
        {

            // Map DTO to entities
            var extendedUser = _mapper.Map<ExtendedUser>(dto);
            var userDetail = _mapper.Map<UserDetails>(dto);

            // Add UserDetail to ExtendedUser
            extendedUser.UserDetails.Add(userDetail);

            // Save ExtendedUser
            var result = await _userManager.CreateAsync(extendedUser, dto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join("\n", result.Errors.Select(e => e.Description)));
            }

            // Sign in user
            await _signInManager.SignInAsync(extendedUser, isPersistent: false);

            // Return success response
            return constructResponse(result);


        }
        public class StatementConfig
        {
            public List<Columns> columns { get; set; }
        }
        public class Columns
        {

            public int index { get; set; }
            public string columnName { get; set; }
            public string selectedToDisplay { get; set; }
        }
    }
}
