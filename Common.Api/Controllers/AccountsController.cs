using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Common.Api.Configurations;
using Common.Api.Models;
using Common.Entities.Models;

namespace Common.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/accounts")]
    public class AccountsController : BaseController
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        readonly JwtOptions _jwtOptions;
        readonly ILogger<AccountsController> _logger;

        readonly JwtTokenCache _tokenCache;

        public AccountsController(UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<Role> roleManager, IOptions<JwtOptions> jwtOptions,
            ILoggerFactory loggerFactory, JwtTokenCache tokenCache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenCache = tokenCache;
            _jwtOptions = jwtOptions.Value;
            _logger = loggerFactory.CreateLogger<AccountsController>();

            ThrowIfInvalidOptions(_jwtOptions);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {

                _logger.LogWarning(LoggingEvents.InvalidAction, "Invalid credentials.", request);
                return Unauthorized();
            }

            try
            {
                User user = await TryLoginAsync(request);

                ICollection<Claim> userClaims = await GetUserClaims(user);

                string encodedJwt = await GenerateToken(request, user, userClaims);

                // Serialize and return the response
                var response = new UserProfile
                {
                    UserName = user.UserName,
                    UserType = user.UserType,
                    AccessList = userClaims.Select(c => c.Value).ToList(),
                    Token = encodedJwt,
                    TokenExpirationDate = DateTime.Now.AddSeconds((int) _jwtOptions.ValidFor.TotalSeconds)
                };

                _logger.LogInformation(LoggingEvents.ValidAction, "User logged in.");
                return Ok(response);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("password")]
        public async Task<ActionResult> ChangePassword([FromForm] string currentPassword, [FromForm] string newPassword)
        {
            string username = GetCurrentUsername();
            User user = await _userManager.FindByNameAsync(username);
            IdentityResult result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                _logger.LogError(LoggingEvents.InvalidAction, "The user could not changed its password.", result);
                return new BadRequestObjectResult(result.Errors.Select(e => e.Code));
            }

            _logger.LogInformation(LoggingEvents.ValidAction, "The user could changed its password.");

            return Ok();
        }

        private async Task<ICollection<Claim>> GetUserClaims(User user)
        {
            ICollection<string> rolNames = await _userManager.GetRolesAsync(user);
            Role rol = await _roleManager.FindByNameAsync(rolNames.FirstOrDefault());
            ICollection<Claim> userClaims = await _roleManager.GetClaimsAsync(rol);

            _logger.LogInformation(LoggingEvents.GetItem, "The user could get its claims.");

            return userClaims;
        }

        private async Task<string> GenerateToken(LoginRequest request, User user, ICollection<Claim> userClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.UserName),
                new Claim("username", request.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64)
            };

            if (user.UserType > 0)
            {
                claims.Add(new Claim(_jwtOptions.CommonClaimName, user.UserType.ToString()));
            }

            claims.AddRange(userClaims.Select(c => new Claim(JwtOptions.ClaimAcessName, c.Type)));

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer : _jwtOptions.Issuer,
                audience : _jwtOptions.Audience,
                claims : claims,
                notBefore : _jwtOptions.NotBefore,
                expires : _jwtOptions.Expiration,
                signingCredentials : _jwtOptions.SigningCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            string encodedJwt = tokenHandler.WriteToken(jwt);

            _tokenCache.Add(jwt.Id);

            _logger.LogInformation(LoggingEvents.GenerateItems, "Generate the Token.");

            return encodedJwt;
        }

        [HttpGet]
        [Route("token-validation-request")]
        public void GetProfile() => NoContent();

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] UserProfile profile)
        {
            string token = await TryGetToken(profile);
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(token))
            {
                _logger.LogWarning(LoggingEvents.InvalidAction, "could not logged out.", profile);
                return Ok();
            }
            var jwtToken = tokenHandler.ReadJwtToken(token);

            _tokenCache.Remove(jwtToken.Id);
            _logger.LogInformation(LoggingEvents.ValidAction, "User logged out.");
            return Ok();
        }

        async Task<string> TryGetToken(UserProfile profile)
        {
            return profile?.Token ?? await HttpContext.GetTokenAsync("access_token");
        }

        static void ThrowIfInvalidOptions(JwtOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {                
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtOptions.JtiGenerator));
            }
        }

        static long ToUnixEpochDate(DateTime date) =>(long) Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        async Task<User> TryLoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.InvalidAction, "Unauthorized to Login.", request);
                throw new UnauthorizedAccessException("InvalidUserNameOrPassword");
            }

            // check the credentials  
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded)
            {
                _logger.LogWarning(LoggingEvents.InvalidAction, "Unauthorized to Login.", request);
                throw new UnauthorizedAccessException("InvalidUserNameOrPassword");
            }

            return user;
        }
    }
}