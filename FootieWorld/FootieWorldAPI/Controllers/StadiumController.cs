using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FootieWorld.data.ef;

namespace FootieWorldAPI.Controllers
{
    public class StadiumController : ApiController
    {
        // GET api/stadium
        [HttpGet]
        
        public List<tblStadium> Get()
        {
            var db = new FootieDbEntities();

            return db.tblStadiums.ToList();
            
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
