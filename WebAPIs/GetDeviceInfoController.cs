using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;
using LEMServer.TcpServer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LEMServer.WebAPIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDeviceInfoController : ControllerBase
    {
        // GET: api/<GetDeviceInfoController>
        /// <summary>
        /// 获取所有当前在线设备信息
        /// </summary>
        /// <returns>设备信息列表</returns>
        [HttpGet]
        public IEnumerable<DeviceInformation> Get()
        {
            MemoryCache cache = MemoryCache.Default;
            List<DeviceInformation> diList = cache.ToList().Select(x => { return (DeviceInformation)x.Value; }).ToList();
            return diList;
        }

        // GET api/<GetDeviceInfoController>/5
        /// <summary>
        /// 获取所有当前在线的某设备信息
        /// </summary>
        /// <param name="deviceid">设备唯一id，此id也是设备内标识</param>
        /// <returns>设备信息</returns>
        [HttpGet("{deviceid}")]
        public DeviceInformation Get(string deviceid)
        {
            MemoryCache cache = MemoryCache.Default;
            DeviceInformation deviceInformation = (DeviceInformation)cache.Get(deviceid);
            return deviceInformation;
        }

        // POST api/<GetDeviceInfoController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<GetDeviceInfoController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<GetDeviceInfoController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
