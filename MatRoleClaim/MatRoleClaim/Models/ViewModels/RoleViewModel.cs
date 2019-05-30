using MatRoleClaim.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatRoleClaim.Models.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public static explicit operator RoleViewModel(ApplicationRole applicationRole)  // explicit byte to digit conversion operator
        {
            RoleViewModel roleViewModel = new RoleViewModel
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name,
                Description = applicationRole.Description,
                Status = true
            };
            return roleViewModel;
        }

    }
}