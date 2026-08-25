using BPM.Web.Identity.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BPM.Web.Identity.API.CustomFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _feature;
        private readonly string _activity;

        public PermissionAttribute(string feature, string activity)
        {
            _feature = feature;
            _activity = activity;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var roleClaim = context.HttpContext.User.FindFirst("RoleId");

            if (roleClaim == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            Guid roleId = Guid.Parse(roleClaim.Value);

            var repository = context.HttpContext.RequestServices
                .GetRequiredService<IPermissionRepository>();

            bool hasPermission = await repository.HasPermissionAsync(
                roleId,
                _feature,
                _activity);

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
