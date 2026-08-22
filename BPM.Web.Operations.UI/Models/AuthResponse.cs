using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Models
{
    public class AuthResponse
    {
        public string Message { get; set; }

        public bool IsValidUser { get; set; }

        public bool IsValidPassword { get; set; }

        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }

        public AuthenticateResponseDto AuthenticateResponseDto { get; set; }
    }
}
