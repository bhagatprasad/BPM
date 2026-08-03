namespace BPM.Web.Distributor.UI.Models.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public Guid? DealerId { get; set; }
        public Guid? RoleId { get; set; }
        public DealerDto DealerInfo { get; set; }
        public RoleDto RoleInfo { get; set; }
    }
}
