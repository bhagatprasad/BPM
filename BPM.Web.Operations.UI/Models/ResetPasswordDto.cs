using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Models
{
    public class ResetPasswordDto
    {
        public Guid UserId { get; set; }

        public string NewPassword { get; set; }
    }
}
