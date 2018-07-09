using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Entities.Models;
using Common.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Common.Data.Seeders
{
    public class DataSeeder
    {
        private readonly DataContext _context;
        readonly IHostingEnvironment _environment;
        readonly UserManager<User> _userManager;
        readonly RoleManager<Role> _roleManager;

        public DataSeeder(DataContext context, IHostingEnvironment environment,
            UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Seed(ISet<Province> provinces)
        {
            _context.Database.Migrate();

            CreateUserAndRoles().Wait();
            CreatePoliticalDivisions(provinces).Wait();
        }

        async Task CreatePoliticalDivisions(ISet<Province> provinces)
        {
            ISet<Province> currentProvinces = _context.Provinces.ToHashSet();
            ISet<Province> intercep = provinces.Where(p => !currentProvinces.Any(c => c.Name == p.Name)).ToHashSet();

            await _context.Provinces.AddRangeAsync(intercep);
            await _context.SaveChangesAsync();
        }

        async Task CreateUserAndRoles()
        {
            if (await _userManager.Users.CountAsync() > 0)
            {
                return;
            }

            using (var scope = await _context.Database.BeginTransactionAsync())
            {
                var role = new Role { Name = "Administrator", Active = true };
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    scope.Rollback();
                    return;
                }

                IEnumerable<Claim> accessList = Enum.GetValues(typeof(AccessList)).Cast<AccessList>()
                    .Select(a => new Claim(a.ToString(), a.ToString()));

                foreach (var access in accessList)
                {
                    var claimResult = await _roleManager.AddClaimAsync(role, access);
                    if (!claimResult.Succeeded)
                    {
                        scope.Rollback();
                        return;
                    }
                }

                ISet<User> currentUser = _context.Users.ToHashSet();

                var user = new HashSet<User>
                {
                    new User {UserName = "manager", FirstName = "manager", LastName = "manager", Email = "manager@invalid.com", UserType = UserType.Administrator, Active = true, Role = role, PasswordHash = "AAbb11"},
                    new User {UserName = "Jirafales", FirstName = "Ruben", LastName = "Aguirre", Email = "jirafales@profesor.com", UserType = UserType.Operator, Active = true, Role = role, PasswordHash = "AAbb11"}
                };

                user = user.Where(c => !currentUser.Any(v => v.Id == c.Id)).ToHashSet();
                foreach (var users in user)
                {
                    await _userManager.CreateAsync(users, "AAbb11");
                    await _userManager.AddToRoleAsync(users, role.Name);
                }

                scope.Commit();
            }
        }
    }
}
