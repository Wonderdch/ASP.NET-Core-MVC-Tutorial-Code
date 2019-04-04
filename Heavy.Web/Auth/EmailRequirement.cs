using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web.Auth
{
    public class EmailRequirement : IAuthorizationRequirement
    {
        public string RequiredEmail { get; set; }

        public EmailRequirement(string requiredEmail)
        {
            RequiredEmail = requiredEmail;
        }
    }
}