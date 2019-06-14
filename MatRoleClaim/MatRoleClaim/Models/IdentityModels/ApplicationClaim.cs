using System.Collections.Generic;

namespace MatRoleClaim.Models.IdentityModels
{
    public class ApplicationClaim
    {
        public ApplicationClaim()
        {
            Roles = new HashSet<ApplicationRole>();
        }

        public string Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ApplicationRole> Roles { get; set; }
    }
}