using AIB.Common;
using AIB.Data.DTOs;
using AIB.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static AIB.Common.Constants;

namespace AIB.Data.Repositories
{
    public class ExtendedRoleRepository: IExtendedRoleRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly RoleManager<ExtendedRole> _roleManager;
        private readonly UserManager<ExtendedUser> _userManager;

        public ExtendedRoleRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<ExtendedRole>>();
            _userManager = _serviceProvider.GetRequiredService<UserManager<ExtendedUser>>();
        }
        public async Task<bool> CreateRoleWithClaims(RoleClaimsDTO model)
        {

            var role = await _roleManager.FindByNameAsync(model.RoleName.Trim());
            if (role == null)
            {
                ExtendedRole extendedRole = new ExtendedRole();
                extendedRole.Name = model.RoleName.Trim();
                var result = await _roleManager.CreateAsync(extendedRole);
                foreach (var claimType in model.ClaimType)
                {
                    var claims = new List<Claim>();
                    if (claimType.ClaimValue.Create)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Create);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.Edit)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Edit);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.View)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.View);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.Delete)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Delete);
                        claims.Add(claim);
                    }

                    foreach (var item in claims)
                    {
                        await _roleManager.AddClaimAsync(extendedRole, item);
                    }
                }
                OtherConstants.isSuccessful = true;
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                OtherConstants.responseMsg = "Role already exsists.";
                return false;
            }
        }

        public async Task<bool> UpdateRoleWithClaims(string roleId, RoleClaimsDTO model)
        {

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                role.Name = model.RoleName.Trim();
                await _roleManager.UpdateAsync(role);
                var claimsList = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claimsList)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
                foreach (var claimType in model.ClaimType)
                {

                    var claims = new List<Claim>();
                    if (claimType.ClaimValue.Create)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Create);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.Edit)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Edit);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.View)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.View);
                        claims.Add(claim);
                    }
                    if (claimType.ClaimValue.Delete)
                    {
                        Claim claim = new Claim(claimType.ClaimTypeName, ClaimValue.Delete);
                        claims.Add(claim);
                    }

                    foreach (var item in claims)
                    {
                        await _roleManager.AddClaimAsync(role, item);
                    }
                }
                OtherConstants.isSuccessful = true;
                return true;
            }
            else
            {
                OtherConstants.isSuccessful = false;
                return false;
            }
        }

        public async Task<IEnumerable<ExtendedRole>> GetAllRoles()
        {
            List<ExtendedRole> list = new List<ExtendedRole>();
            var rolesList = _roleManager.Roles.ToList();
            foreach (var item in rolesList)
            {
                ExtendedRole role = _mapper.Map<ExtendedRole>(item);
                var usersList = await _userManager.GetUsersInRoleAsync(item.Name);
                list.Add(role);
            }
            return list;
        }

        public async Task<bool> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if ((await _roleManager.DeleteAsync(role)).Succeeded)
            {
               return OtherConstants.isSuccessful = true;
            }
            return OtherConstants.isSuccessful = false;
        }

        public async Task<RoleClaimsDTO> GetClaimsAgainstRole(string roleId)
        {
            RoleClaimsDTO roleClaimsDTO = new RoleClaimsDTO();
            var role = await _roleManager.FindByIdAsync(roleId);
            roleClaimsDTO.RoleName = role.Name;
            roleClaimsDTO.ClaimType = new List<ClaimTypeDTO>();
            if (role != null)
            {
                var claimsList = await _roleManager.GetClaimsAsync(role);

                foreach (var claimType in Utils.GetClaimTypes())
                {
                    var claims = claimsList.Where(x => x.Type == claimType);
                    roleClaimsDTO.ClaimType.Add(MapClaimTypeWithValues(claims, claimType));

                }
            }
            OtherConstants.isSuccessful = true;
            return roleClaimsDTO;
        }
        private ClaimTypeDTO MapClaimTypeWithValues(IEnumerable<Claim> claims, string claimtype)
        {
            ClaimTypeDTO claimTypeDTO = new ClaimTypeDTO();
            claimTypeDTO.ClaimTypeName = claimtype;
            claimTypeDTO.ClaimValue = new ClaimValueDTO();
            if (claims.Count() > 0)
            {
                foreach (var claim in claims)
                {
                    if (claim.Value == ClaimValue.Create)
                    {
                        claimTypeDTO.ClaimValue.Create = true;
                    }
                    if (claim.Value == ClaimValue.View)
                    {
                        claimTypeDTO.ClaimValue.View = true;
                    }
                    if (claim.Value == ClaimValue.Edit)
                    {
                        claimTypeDTO.ClaimValue.Edit = true;
                    }
                    if (claim.Value == ClaimValue.Delete)
                    {
                        claimTypeDTO.ClaimValue.Delete = true;
                    }
                }
            }
            return claimTypeDTO;
        }
    }

    public interface IExtendedRoleRepository
    {
        Task<bool> CreateRoleWithClaims(RoleClaimsDTO model);
        Task<bool> UpdateRoleWithClaims(string roleId, RoleClaimsDTO model);
        Task<IEnumerable<ExtendedRole>> GetAllRoles();
        Task<bool> DeleteRole(string roleId);


    }
}
