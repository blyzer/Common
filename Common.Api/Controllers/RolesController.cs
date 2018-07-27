using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Common360.Api.Models;
using Common360.Core.Rules;
using Common360.Entities.Models;

namespace Common360.Api.Controllers
{
    [Route("api/roles")]
    public class RolesController : Controller
    {
        private readonly RoleRules _roleRules;

        public RolesController(RoleRules roleRules)
        {
            _roleRules = roleRules;
        }

        [HttpGet]
        [Route("")]
        public ICollection<RolViewModel> GetAll(int? pageNumber, int? pageSize)
        {
            int size = pageSize.GetValueOrDefault(25);
            int number = pageNumber.GetValueOrDefault(0);
            ICollection<Role> roles = _roleRules.GetAll(number, size);
            return roles
                .Select(r => new RolViewModel
                {
                    Name = r.Name,
                        Active = r.Active,

                }).ToList();
        }

        [HttpGet]
        [Route("{roleName}")]
        public async Task<RolViewModel> GetAsync(string roleName)
        {
            Role role = await _roleRules.GetAsync(roleName);

            return new RolViewModel
            {
                Name = role.Name,
                    Active = role.Active,
                    Access = await _roleRules.GetClamimsAsync(role)
            };
        }

        [HttpPost]
        [Route("")]
        public async Task CreateAsync([FromBody] RolViewModel value) => await PersistAsync(value, true);

        [HttpPut]
        [Route("{roleName}")]
        public async Task EditAsync([FromBody] RolViewModel value) => await PersistAsync(value, false);

        [Route("{code}/code-validation")]
        [HttpGet]
        public async Task<CustomValidationResult> ValidateCodeAsync(string code)
        {
            Role rol = await _roleRules.GetAsync(code);
            return new CustomValidationResult(rol != null);
        }

        private async Task PersistAsync(RolViewModel value, bool isNew)
        {
            var role = new Role { Name = value.Name, Active = value.Active };

            if (isNew)
            {
                await _roleRules.CreateAsync(role, value.Access);
            }
            else
            {
                await _roleRules.EditAsync(role, value.Access);
            }
        }
    }
}