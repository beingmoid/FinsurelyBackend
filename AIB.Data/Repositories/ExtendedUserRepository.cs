using AIB.Common.EmailService;
using AIB.Data.DTOs;
using AIB.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AIB.Common.Constants;

namespace AIB.Data.Repositories
{
    public class ExtendedUserRepository : IExtendedUserRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly AIBContext _dbContext;

        public ExtendedUserRepository(IServiceProvider serviceProvider , AIBContext dbContext)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _dbContext = dbContext;
        }

        public async Task<List<ExtendedUser>> GetTeamMembers()
        {
            return await  _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>().Users.Where(x=>x.TypeOfUser==TypeOfUser.Broker).ToListAsync();
        }
        public async Task<bool> CheckIsEmailExist(string Email)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>();
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                OtherConstants.isSuccessful = true;
            else
                OtherConstants.isSuccessful = false;

            return OtherConstants.isSuccessful;
        }
        private string CreateEmailTemplate(string link, string description, string buttonTitle, string message, string address)
        {
            var _env = _serviceProvider.GetRequiredService<IHostingEnvironment>();
            string path = Path.Combine(_env.ContentRootPath, "EmailTemplates\\EmailTemplateForVerifyAndReset.html");
            if (System.IO.File.Exists(path))
            {
                string html = System.IO.File.ReadAllText(path);
                html = html.Replace("{logoLink}", LogoConfigurations.PortalFrontEndLogo);
                html = html.Replace("{description}", description);
                html = html.Replace("{link}", link);
                html = html.Replace("{buttonTitle}", buttonTitle);
                html = html.Replace("{message}", message);
                html = html.Replace("{address}", address);
                return html;
            }
            return null;
        }
        public async Task<bool> CreateTeamMember(TeamMemberDTO user)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>();
            var teamMember = _mapper.Map<ExtendedUser>(user);
            teamMember.TypeOfUser = TypeOfUser.Broker;
            teamMember.UserName = teamMember.Email;
            teamMember.Id = Guid.NewGuid().ToString();
            teamMember.Password = null;
            teamMember.PasswordHash = null;


            var result = await _userManager.CreateAsync(teamMember);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(teamMember, user.Role);
                var url = DomainConfiguration.PortalAppDomain + $"#/auth/register-user?userid={teamMember.Id}";
                var body = CreateEmailTemplate(url, EmailTemplateConfiguration.VerifyEmailAddress, EmailTemplateConfiguration.VerifyEmailButtonTitle, EmailTemplateConfiguration.VerifyEmailMessage, EmailTemplateConfiguration.ResetEmailAddress);


                var isEmailSent = _serviceProvider.GetRequiredService<IEmailService>().SendEmailWithoutTemplate(
                    user.Email,
                      user.FirstName + " " + user.LastName,
                    "Email Verification Notification From NukesLab LLC",
                    body, true);
                if (isEmailSent)
                    OtherConstants.isSuccessful = true;
                if (teamMember.isAgent)
                {
                    var agent = new Agent();
                    agent.Name = teamMember.FirstName+" "+teamMember.LastName;
                    agent.BranchId=teamMember.BranchId;
                    agent.OpeningBalance = 0;

                    var agentServic = _serviceProvider.GetRequiredService<IAgentRepository>();
                    await agentServic.Insert(new[] { agent });
                    var savedAgent = await agentServic.SaveChanges();
                    OtherConstants.isSuccessful = savedAgent;
                    return savedAgent ? true : false;
                }
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                return false;
            }


        }

        public async Task<bool> DeleteTeamMember(string id)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>();
            var result = await _userManager.DeleteAsync(await _userManager.FindByIdAsync(id));
            if (result.Succeeded)
            {
                OtherConstants.isSuccessful = true;
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                return false;
            }
        }
        public async Task<bool> OnBoardingMember(OnBoarding model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>();
            var user = await _userManager.FindByIdAsync(model.id);
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            if (result.Succeeded)
            {

                user.EmailConfirmed = true;
                var res = await _userManager.UpdateAsync(user);
                OtherConstants.isSuccessful = true;
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                OtherConstants.responseMsg += result.Errors.Select(x => x.Description).FirstOrDefault();
                return false;
            }
        }
        public async Task<bool> UpdateTeamMember(string id, TeamMemberDTO model)
        {
            var _userManager = _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>();
            var user = await _userManager.FindByIdAsync(model.Id);
            this._mapper.Map(model,user);
            user.TypeOfUser = TypeOfUser.Broker;
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            if (result.Succeeded )
            {
                user.EmailConfirmed = true;
                var res = await _userManager.UpdateAsync(user);
                OtherConstants.isSuccessful = true;
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                return false;
            }
        }
    }
    public interface IExtendedUserRepository
    {
        Task<bool> CreateTeamMember(TeamMemberDTO model);
        Task<bool> UpdateTeamMember(string id, TeamMemberDTO user);
        Task<bool> DeleteTeamMember(string id);
        Task<bool> CheckIsEmailExist(string Email);
        Task<List<ExtendedUser>> GetTeamMembers();
        Task<bool> OnBoardingMember(OnBoarding model);

    }
    public class OnBoarding
    {
        public string id { get; set; }
        public string Password { get; set; }
    }
}
