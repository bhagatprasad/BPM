namespace BPM.Web.Identity.API.Models.DTOs
{
    public class ActivityUpdateDto
    {
        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public Guid? ModifiedBy { get; set; }
    }
}
