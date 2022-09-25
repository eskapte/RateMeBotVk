using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateMeBotVk
{
    public class AppSettings
    {
        public uint LongPollVersion { get; set; }
        public ulong GroupId { get; set; }
        public int OnErrorDelay { get; set; }
    }
}
