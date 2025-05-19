using System.IdentityModel.Tokens.Jwt;
using InRoom.DLL.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InRoom.API.Helpers;

public class RoleVerificationAttribute : Attribute, IAuthorizationFilter
{
    private readonly Roles _role;

    public RoleVerificationAttribute(Roles role)
    {
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();

        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var jwtToken = jwtHandler.ReadJwtToken(token);
         
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (roleClaim != _role.ToString())
            {
                context.Result = new ForbidResult();
                return;
            }
        }
        catch (Exception)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}