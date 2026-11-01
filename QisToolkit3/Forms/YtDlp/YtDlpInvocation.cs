using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QisToolkit3.Forms.YtDlp
{
    public class YtDlpInvocation
    {
        public string Url { get; set; }
        public Dictionary<string, string> Overrides { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        public bool AutoDownload { get; set; } = true;
        public string CookieFilePath { get; set; }
    }
}
