namespace BPM.Web.API.Models.DTOs
{
    public class UserDistributorUpdateDto
    {
        public Guid UserId { get; set; }
        public Guid? DistributorId { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
