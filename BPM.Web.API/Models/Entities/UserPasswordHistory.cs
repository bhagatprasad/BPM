using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("user_password_history")]
    public class UserPasswordHistory
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("userid")]
        public Guid UserId { get; set; }

        [Required]
        [Column("passwordhash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Column("passwordsalt")]
        public string PasswordSalt { get; set; } = string.Empty;

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}