using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatRoleClaim.Models.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}