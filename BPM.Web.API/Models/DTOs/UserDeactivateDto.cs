namespace BPM.Web.API.Models.DTOs
{
    public class UserDeactivateDto
    {
        public Guid UserId { get; set; }
        //public bool IsActive { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
