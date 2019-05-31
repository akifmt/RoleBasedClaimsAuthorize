using MatRoleClaim.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatRoleClaim.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
    }
}