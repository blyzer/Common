using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common360.Core.Repositories;
using Common360.Entities.Models;

namespace Common360.Core.Rules
{
    public sealed class UserRules : RulesBase<User>
    {
        readonly UserRepository _userRepository;

        public UserRules(UserRepository UserRepository) : base(UserRepository)
        {
            _userRepository = UserRepository;
        }

        public ICollection<User> GetPaged(int count, int pageNumber, string schoolCode) => _userRepository.GetPaged(count, pageNumber, schoolCode).ToList();

        public async Task<User> GetAsync(string userName) => await _userRepository.GetAsync(userName);

        public async Task<ICollection<string>> GetUserRoleNamesAsync(string roleName) => await _userRepository.GetUserRoleNamesAsync(roleName);

        public async Task CreateUserAsync(User user, string password)
        {
            await _userRepository.CreateUser(user, password);
            await _userRepository.SaveChangesAsync();
        }

        public async Task EditUserAsync(User updatedUser, string password)
        {
            await _userRepository.UpdateUserAsync(updatedUser, password);
            await _userRepository.SaveChangesAsync();
        }
    }
}