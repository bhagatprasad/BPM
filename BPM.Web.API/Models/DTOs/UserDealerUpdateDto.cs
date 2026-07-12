namespace BPM.Web.API.Models.DTOs
{
    public class UserDealerUpdateDto
    {
        public Guid UserId { get; set; }

        public Guid DealerId { get; set; }

        public Guid ModifiedBy { get; set; }
    }
}
