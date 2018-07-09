using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Data;
using Common.Entities.Models;
using Common.Entities.Enums;

namespace Common.Core.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserRepository(DataContext context, UserManager<User> userManager, RoleManager<Role> roleManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        internal IQueryable<User> GetPaged(int count, int pageNumber, string currentSchool)
        {
            IQueryable<User> allUsers = _userManager.Users.Include(u => u.Role)
                .Where(u => u.UserType == UserType.Operator);
            
            return allUsers;
        }

        public override async Task<User> GetAsync(params object[] keyValues)
        {
            User user = await _userManager.FindByNameAsync(keyValues.First().ToString());
            
            var roleNames = await _userManager.GetRolesAsync(user);
            user.Role = new Role {Name = roleNames.First()};
            return user;
        }

        public async Task<IList<string>> GetUserRoleNamesAsync(string username) => await _userManager.GetRolesAsync(new User { UserName = username });

        public async Task CreateUser(User user, string password)
        {
            string roleName = user.Role.Name;
            user.Role = _roleManager.Roles.FirstOrDefault(r => r.Name == roleName);

            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException();
            }

            await _userManager.AddToRoleAsync(user, roleName);
        }

        internal async Task UpdateUserAsync(User updatedUser, string password)
        {
            User user = await _userManager.FindByNameAsync(updatedUser.UserName);

            if (user == null)
            {
                throw new ArgumentNullException("UserNotFound");
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Active = updatedUser.Active;

            await _userManager.UpdateAsync(user);

            IList<string> roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            await _userManager.AddToRoleAsync(user, updatedUser.Role.Name);

            if (!string.IsNullOrWhiteSpace(password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, password);
            }
        }
    }
}