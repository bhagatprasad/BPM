namespace BPM.Web.API.Models.DTOs
{
    public class UserActivateDto
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
