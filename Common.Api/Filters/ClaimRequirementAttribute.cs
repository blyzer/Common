using Common.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Common.Api.Filters
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, params AccessList[] claimValues): base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { claimType, claimValues };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly string _claimType;
        readonly AccessList[] _claimValues;

        public ClaimRequirementFilter(string claimType, params AccessList[] claimValues)
        {
            _claimType = claimType;
            _claimValues = claimValues;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims
                .Any(c => c.Type == _claimType && _claimValues.Any(cv => cv.ToString()== c.Value));

            if (!hasClaim)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}