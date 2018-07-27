using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Common360.Entities.Models;

namespace Common360.Core.Rules
{
    public class RoleRules
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleRules(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public ICollection<Role> GetAll(int pageNumber, int pageSize)
        {
            var roles = _roleManager.Roles.Take(pageSize).Skip(pageSize * pageNumber).ToList();
            return roles;
        }

        public async Task<Role> GetAsync(string roleName) => await _roleManager.FindByNameAsync(roleName);

        public async Task CreateAsync(Role role, ICollection<string> accessList)
        {
            await _roleManager.CreateAsync(role);
            await SetClaims(role, accessList);
        }

        public async Task EditAsync(Role role, ICollection<string> accessList)
        {
            Role currentRole = await _roleManager.FindByNameAsync(role.Name);
            currentRole.Active = role.Active;
            IdentityResult result = await _roleManager.UpdateAsync(currentRole);
            if (!result.Succeeded)
            {
                throw new System.ArgumentException(string.Join(",", result.Errors.Select(e => e.Code)));
            }
            await SetClaims(currentRole, accessList);
        }

        private async Task SetClaims(Role role, ICollection<string> accessList)
        {
            IList<Claim> claims = await _roleManager.GetClaimsAsync(role);

            foreach (Claim claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            foreach (string access in accessList)
            {
                IdentityResult result = await _roleManager.AddClaimAsync(role, new Claim(access, access));
            }
        }

        public async Task<ICollection<string>> GetClamimsAsync(Role role)
        {
            ICollection<Claim> claims = await _roleManager.GetClaimsAsync(role);
            return claims.Select(c => c.Type).ToHashSet();
        }
    }
}