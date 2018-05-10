using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Footieworld.core;
using FootieWorld.data.ef;

namespace FootieWorldApi.Controllers
{
    public class StadiumController : ApiController
    {
        // GET api/stadium
        public IEnumerable<string> Get()
        {
            var db = new FootieDbEntities();
            var stadiums = db.tblTeams.Select(o => o.Name).ToList();
            return stadiums;
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
