using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MatRoleClaim.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MatRoleClaim.Models.IdentityModels
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
            Claims = new HashSet<ApplicationClaim>();
        }
        public ApplicationRole(string name) : base(name)
        {
            Claims = new HashSet<ApplicationClaim>();
        }
        public string Description { get; set; }

        public virtual ICollection<ApplicationClaim> Claims { get; set; }

    }
}