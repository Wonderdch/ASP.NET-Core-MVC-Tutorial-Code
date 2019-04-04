using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web.Auth
{
    public class EmailHandler : AuthorizationHandler<EmailRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            EmailRequirement requirement)
        {
            var claim = context.User.Claims.FirstOrDefault(c => c.Type == "Email");

            if (claim != null && claim.Value.EndsWith(requirement.RequiredEmail))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}