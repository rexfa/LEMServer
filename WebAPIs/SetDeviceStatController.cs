using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LEMServer.WebAPIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetDeviceStatController : ControllerBase
    {
        // GET: api/<SetDeviceStatController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<SetDeviceStatController>/5
        /// <summary>
        /// 设置设备状态
        /// </summary>
        /// <param name="deviceid">设备id</param>
        /// <param name="devicestat">状态id</param>
        /// <returns></returns>
        [HttpGet("{deviceid}/{devicestat}")]
        public string Get(string deviceid,int devicestat)
        {
            return deviceid + "set:" + devicestat;
        }

        // POST api/<SetDeviceStatController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<SetDeviceStatController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<SetDeviceStatController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
