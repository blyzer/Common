using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Common.Api.Configurations;
using Common.Api.Filters;
using Common.Api.Models;
using Common.Core.Rules;
using Common.Entities.Models;
using Common.Entities.Enums;

namespace Common.Api.Controllers
{

    [Route("api/users")]
    public class UsersController : BaseController
    {
        readonly ILogger<AccountsController> _logger;
        readonly UserRules _rules;

        public UsersController(UserRules rules, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AccountsController>();
            _rules = rules;
        }

        [HttpGet]
        [Route("")]
        public ICollection<UserViewModel> Get([FromQuery] int count, [FromQuery] int pageNumber)
        {
            string currentPermission = GetCurrentPermission();
            ICollection<User> allUsers = _rules.GetPaged(count, pageNumber, currentPermission);

            _logger.LogInformation(LoggingEvents.GetItem, "Getting a User.");

            return allUsers.ToList()
                .Map<ICollection<User>, ICollection<UserViewModel>>();
        }


        [HttpGet]
        [Route("{userName}")]
        [ClaimRequirement(JwtOptions.ClaimAcessName, AccessList.Users)]
        public async Task<UserViewModel> GetAsync(string userName)
        {
            User user = await _rules.GetAsync(userName);

            var userModel = user.Map<User, UserViewModel>();

            _logger.LogInformation(LoggingEvents.GetItem, "Getting a User Async.");

            return userModel;
        }

        [HttpPost]
        [Route("")]
        [ClaimRequirement(JwtOptions.ClaimAcessName, AccessList.Users)]
        public async Task<IActionResult> Create([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.GetItem, "Could not Create the user.", model);
                return BadRequestModelState();
            }

            User user = model.Map<UserViewModel, User>();

            await _rules.CreateUserAsync(user, model.Password);

            _logger.LogInformation(LoggingEvents.GetItem, "Creating a user.");

            return Ok();
        }

        [HttpPut]
        [Route("{userName}")]
        [ClaimRequirement(JwtOptions.ClaimAcessName, AccessList.Users)]
        public async Task<IActionResult> Edit(string userName, [FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.GetItem, "Could not Edit the user.", model);
                return BadRequestModelState();
            }

            User user = model.Map<UserViewModel, User>();

            await _rules.EditUserAsync(user, model.Password);

            _logger.LogInformation(LoggingEvents.GetItem, "Editing a user.");

            return Ok();
        }

        [Route("{username}/code-validation")]
        [HttpGet]
        public async Task<CustomValidationResult> ValidateCodeAsync(string username)
        {
            User user = await _rules.GetAsync(username);
            return new CustomValidationResult(user != null);
        }
    }
}