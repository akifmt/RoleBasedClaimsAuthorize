using MatRoleClaim.Models.IdentityModels;

namespace MatRoleClaim.Models.IdentityModels
{
    public class ApplicationRoleClaim
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public string ClaimId { get; set; }
        public ApplicationClaim Claim { get; set; }
    }
}