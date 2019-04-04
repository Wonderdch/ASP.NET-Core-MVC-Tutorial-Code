using System.Collections.Generic;

namespace Heavy.Web.Data
{
    public static class ClaimTypes
    {
        public static List<string> AllClaimTypeList { get; set; } = new List<string>
        {
            "Edit Albums",
            "Edit Users",
            "Edit Roles",
            "Email"
        };
    }
}