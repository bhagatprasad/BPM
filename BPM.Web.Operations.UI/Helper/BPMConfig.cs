using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.Web.Operations.UI.Helper
{
    public class BPMConfig
    {
        public string ApplicationName { get; set; }
        public string Version { get; set; }
        public string BaseUrl { get; set; }
        public string RedirectUri { get; set; }
        public int TimeoutSeconds { get; set; } = 60;
        public int RetryCount { get; set; } = 3;
        public int RetryDelaySeconds { get; set; } = 2;
    }
}
