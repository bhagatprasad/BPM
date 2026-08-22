using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Models
{
    public class ApplicationUser
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public Guid RoleId { get; set; }

        public Guid? DistributorId { get; set; }
        public bool IsActive { get; set; }

        public string JwtToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
