using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Common.Api.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult BadRequestModelState()
        {
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }
        protected string GetCurrentPermission() => HttpContext.User.Claims.Where(c => c.Type == "Prst_sopa-trunk").Select(c => c.Value).FirstOrDefault();
        protected string GetCurrentUsername() => HttpContext.User.Claims.Where(c => c.Type == "username").Select(c => c.Value).FirstOrDefault();

    }
}