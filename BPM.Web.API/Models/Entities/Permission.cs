using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPM.Web.API.Models.Entities
{
    [Table("permissions")]
    public class Permission
    {
        [Key]
        [Column("permissionid")]
        public Guid PermissionId { get; set; }

        [Column("roleid")]
        public Guid RoleId { get; set; }

        [Column("featureid")]
        public Guid FeatureId { get; set; }

        [Column("activityid")]
        public Guid ActivityId { get; set; }

        [Column("isenabled")]
        public bool IsEnabled { get; set; }

        [Column("createdby")]
        public Guid? CreatedBy { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedby")]
        public Guid? ModifiedBy { get; set; }

        [Column("modifiedon")]
        public DateTime? ModifiedOn { get; set; }
    }
}
