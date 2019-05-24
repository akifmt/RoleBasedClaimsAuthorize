namespace MatRoleClaim.Models
{
    public class RoleClaim
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public string ClaimId { get; set; }
        public ApplicationClaim Claim { get; set; }
    }
}