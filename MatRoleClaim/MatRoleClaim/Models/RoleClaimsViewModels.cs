using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatRoleClaim.Models
{
    public class RoleClaimsViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public List<ClaimViewModel> Claims { get; set; }
    }

    public class ClaimViewModel
    {
        public string ClaimId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}