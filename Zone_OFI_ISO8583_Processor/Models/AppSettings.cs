using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zone_OFI_ISO8583_Processor.Models
{
    internal class AppSettings
    {
        public string ZNIpAddress { get; set; }
        public string ZNPort { get; set; }
        public string APIKey { get; set; }
        public string PushJournalHostAddress { get; set; }
        public int PushJournalHostPort { get; set; }
        public string PushJournalEndPoint { get; set; }
    }
}
