using System.Collections.Generic;

namespace MatRoleClaim.Models
{
    public class ApplicationClaim
    {
        public string Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }
}