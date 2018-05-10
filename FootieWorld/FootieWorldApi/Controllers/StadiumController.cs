using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Footieworld.core;
using Footieworld.core.Models;
using Footieworld.core.Services.Interfaces;
using FootieWorld.data.ef;

namespace FootieWorldApi.Controllers
{
    public class StadiumController : ApiController
    {
        private IStadiumService _stadiumService;

        public StadiumController (IStadiumService stadiumService )
        {
            _stadiumService = stadiumService;
        }


        // GET api/stadium
        public List<Stadium> Get()
        {
            //oldway
            //var db = new FootieDbEntities();
            //var stadiums = db.tblTeams.Select(o => o.Name).ToList();
            //return stadiums;


            return _stadiumService.GetAllStadiums();
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
