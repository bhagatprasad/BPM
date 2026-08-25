namespace BPM.Web.Identity.API.Models.DTOs
{
    public class ActivityCreateDto
    {
        public string ActivityName { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
