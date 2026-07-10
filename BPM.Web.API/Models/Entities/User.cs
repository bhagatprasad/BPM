using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("firstname")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Column("lastname")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("phone")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [Column("roleid")]
        public Guid RoleId { get; set; }

        [Column("dealerid")]
        public Guid? DealerId { get; set; }

        [Required]
        [Column("passwordhash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Column("passwordsalt")]
        public string PasswordSalt { get; set; } = string.Empty;

        [Column("lastlogin")]
        public DateTime? LastLogin { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; } = true;

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }
    }
}