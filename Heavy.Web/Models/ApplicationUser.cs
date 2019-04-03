using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Heavy.Web.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(18)]
        public string IdCard { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}