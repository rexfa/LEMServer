using LEMServer.TcpServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;


namespace LEMServer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string Message { get; private set; } = "No Device ";

        public List<string> DeviceInfoList { get; private set; } = new List<string>();
        public string ServerStat { get; private set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            MemoryCache cache = MemoryCache.Default;
            
            if(cache.Count()>0)
            {
                Message = cache.Count() + " device(s) have been registered";
                foreach (var oc in cache.Distinct()) 
                { 
                    DeviceInformation deviceInformation = (DeviceInformation)oc.Value;
                    DeviceInfoList.Add("ID: " + deviceInformation.DeviceID + "-IP: " + deviceInformation.IP + " -DeviceStat: " + deviceInformation.DeviceStat + " -PowerStat: " + deviceInformation.PowerStat.ToString("00") + " -Ver: " + deviceInformation.DeviceVer);
                }
            }

        }
    }
}
