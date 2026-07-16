using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("userloginhistory")]
    public class UserLoginHistory
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Column("login_time")]
        public DateTime LoginTime { get; set; }

        [Column("logout_time")]
        public DateTime? LogoutTime { get; set; }

        [Column("is_login_successful")]
        public bool IsLoginSuccessful { get; set; }

        [MaxLength(250)]
        [Column("failure_reason")]
        public string? FailureReason { get; set; }

        [MaxLength(50)]
        [Column("ip_address")]
        public string? IpAddress { get; set; }

        [Column("user_agent")]
        public string? UserAgent { get; set; }

        [MaxLength(100)]
        [Column("browser_name")]
        public string? BrowserName { get; set; }

        [MaxLength(100)]
        [Column("operating_system")]
        public string? OperatingSystem { get; set; }

        [MaxLength(200)]
        [Column("device_name")]
        public string? DeviceName { get; set; }

        [MaxLength(200)]
        [Column("location")]
        public string? Location { get; set; }

        [MaxLength(150)]
        [Column("jwt_token_id")]
        public string? JwtTokenId { get; set; }

        [Column("session_id")]
        public Guid? SessionId { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
