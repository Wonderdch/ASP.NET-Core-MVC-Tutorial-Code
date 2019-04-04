using System.Collections.Generic;

namespace Heavy.Web.ViewModels
{
    public class ManageClaimsViewModel
    {
        public string UserId { get; set; }

        public string ClaimId { get; set; }

        public List<string> AllClaims { get; set; }
    }
}