namespace BPM.Web.API.Models.DTOs
{
    public class AuthenticateResponseDto
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Guid? DealerId { get; set; }

        public Guid RoleId { get; set; }

        public bool IsActive { get; set; }

        public DealerDto DealerInfo { get; set; }

    }
}
