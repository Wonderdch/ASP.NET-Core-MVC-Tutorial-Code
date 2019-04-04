using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web.Auth
{
    public class CanEditAlbumHandler : AuthorizationHandler<QualifiedUserRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            QualifiedUserRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "Edit Albums"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}