using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatRoleClaim.Models.ViewModels
{
    public class RoleClaimsViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }


        public List<ClaimViewModel> Claims { get; set; }
    }
}