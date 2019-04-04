using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Heavy.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(18)]
        public string IdCard { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public ICollection<IdentityUserToken<string>> Tokens { get; set; }

        public ICollection<IdentityUserRole<string>> UserRoles { get; set; }

        public ApplicationUser()
        {
            Claims = new List<IdentityUserClaim<string>>();
            Logins = new List<IdentityUserLogin<string>>();
            Tokens = new List<IdentityUserToken<string>>();
            UserRoles = new List<IdentityUserRole<string>>();
        }
    }
}