namespace BPM.Web.API.Models.DTOs
{
    public class UserUpdateDto
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public Guid ModifiedBy { get; set; }
    }
}
