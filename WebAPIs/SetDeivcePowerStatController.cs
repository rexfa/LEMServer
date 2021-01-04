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
    public class SetDeivcePowerStatController : ControllerBase
    {
        // GET: api/<SetDeivcePowerStatController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<SetDeivcePowerStatController>/5
        /// <summary>
        /// 设置设备管理的电源
        /// </summary>
        /// <param name="deviceid">设备信息</param>
        /// <param name="devicepowerstat">设备电源0是关，1是开</param>
        /// <returns></returns>
        [HttpGet("{deviceid}/{devicepowerstat}")]
        public string Get(string deviceid, int devicepowerstat)
        {
            return deviceid + "set:" + devicepowerstat;
        }

        // POST api/<SetDeivcePowerStatController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<SetDeivcePowerStatController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<SetDeivcePowerStatController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
