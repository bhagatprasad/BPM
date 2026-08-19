namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class AuthenticateResponseDto
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
             
        public Guid? DistributorId { get; set; }
        public Guid RoleId { get; set; }

        public bool IsActive { get; set; }        
        public DistributorDto DistributorInfo { get; set; }
        public RoleDto RoleInfo { get; set; }
    }
}