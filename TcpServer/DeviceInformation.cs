using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LEMServer.TcpServer
{
    [Serializable]
    public class DeviceInformation
    {
        public string DeviceID { get; set; }
        public int DeviceStat { get; set; }
        public int PowerStat { get; set; }
        public DateTime InformationUpdatedOn { get; set; }
        public string DeviceVer { get; set; }
        public string IP { get; set; }
        public int port { get; set; } = 9906;
    }
}
