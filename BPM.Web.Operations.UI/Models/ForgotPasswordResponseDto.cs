using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Models
{
    public class ForgotPasswordResponseDto
    {
        public bool Success { get; set; }

        public Guid? UserId { get; set; }

        public string Message { get; set; }
    }
}
