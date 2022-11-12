
using PanoramBackend.Services.Data.DTOs;
using PanoramBackend.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NukesLab.Core.Common.EmailService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static NukesLab.Core.Common.Constants;

namespace PanoramBackend.Data.Repository
{
    public class ExtendedUserRepository : IExtendedUserRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly AMFContext _dbContext;

        public ExtendedUserRepository(IServiceProvider serviceProvider , AMFContext dbContext)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _dbContext = dbContext;
        }

        public async Task<ExtendedUser> GetUser()
        {
            var userId = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User.FindFirstValue("UserId");

            var user = _dbContext.Set<ExtendedUser>().Include(x => x.UserDetails).Where(x=>x.Id==userId).SingleOrDefault();
            return user;
        }

        public async Task<List<ExtendedUser>> GetTeamMembers()
        {
            return await  _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>().Users.ToListAsync();
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

            teamMember.UserDetails=user.UserDetails;
            var result = await _userManager.CreateAsync(teamMember, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(teamMember, user.Role);
                //var otp = _userManager.GenerateTwoFactorTokenAsync(user,)
                var url = DomainConfiguration.PortalAppDomain + $"#/auth/register-user?userid={teamMember.Id}";
                var body = CreateEmailTemplate(url, EmailTemplateConfiguration.VerifyEmailAddress, EmailTemplateConfiguration.VerifyEmailButtonTitle, EmailTemplateConfiguration.VerifyEmailMessage, EmailTemplateConfiguration.ResetEmailAddress);


                var isEmailSent = _serviceProvider.GetRequiredService<IEmailService>().SendEmailWithoutTemplate(teamMember.Email, "Verify Email", body, true);
                if (isEmailSent)
                    OtherConstants.isSuccessful = true;
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                OtherConstants.messageType = MessageType.Error;
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
            //if (user.UserDetails.Count<0)
            //{
            //    user.UserDetails.Add(model.UserDetails);
            //}
            this._mapper.Map(model,user);
           // user.TypeOfUser = TypeOfUser.Broker;
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            if (result.Succeeded )
            {
                user.EmailConfirmed = true;
                var res = await _userManager.UpdateAsync(user);
                //if (model.Loan.Id>0)
                //{
                //    var loanRepo = _serviceProvider.GetRequiredService<LoanRepository>();
                //    model.Loan.ApplicantId = user.Id;
                //    await loanRepo.Insert(new[] { model.Loan });
                //}
                //else
                //{
                //    var loanRepo = _serviceProvider.GetRequiredService<LoanRepository>();
                //     loanRepo.Update(model.Loan.Id,model.Loan);
                //}
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                return false;
            }
        }

        //public async Task<bool> MonoConnected(string monoId)
        //{

        //    var userId = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User.FindFirstValue("UserId");

        //    var user = _dbContext.Set<ExtendedUser>().Include(x => x.UserDetails).Where(x => x.Id == userId).SingleOrDefault();
        //    user.MonoCode = monoId;
        //   _dbContext.Set<ExtendedUser>().Update(user);
        //    var success = (await _dbContext.SaveChangesAsync() > 0);


        //    return success;
        //}
        
    }
    public interface IExtendedUserRepository
    {
        Task<bool> CreateTeamMember(TeamMemberDTO model);
        Task<bool> UpdateTeamMember(string id, TeamMemberDTO user);
        Task<bool> DeleteTeamMember(string id);
        Task<bool> CheckIsEmailExist(string Email);
        Task<List<ExtendedUser>> GetTeamMembers();
        Task<bool> OnBoardingMember(OnBoarding model);
        Task<ExtendedUser> GetUser();
        //Task<bool> MonoConnected(string monoId);

    }
    public class OnBoarding
    {
        public string id { get; set; }
        public string Password { get; set; }
    }
}
