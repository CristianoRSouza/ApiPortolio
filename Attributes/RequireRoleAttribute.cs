using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ApiEntregasMentoria.Interfaces.Services;
using System.Security.Claims;

namespace ApiEntregasMentoria.Attributes
{
    public class RequireRoleAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _roleName;

        public RequireRoleAttribute(string roleName)
        {
            _roleName = roleName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var roleService = context.HttpContext.RequestServices.GetRequiredService<IRoleService>();
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var hasRole = await roleService.HasRoleAsync(userId, _roleName);
            
            if (!hasRole)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}