namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class ActivityDto
    {
        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}