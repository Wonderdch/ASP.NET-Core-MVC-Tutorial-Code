using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Heavy.Web.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }

        public List<IdentityUser> Users { get; set; }

        public UserRoleViewModel()
        {
            Users = new List<IdentityUser>();
        }
    }
}