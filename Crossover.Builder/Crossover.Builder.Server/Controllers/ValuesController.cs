﻿using System.Collections.Generic;
using System.Web.Http;
using Crossover.Builder.Server.Filters;
using Crossover.Builder.Server.Utils;

namespace Crossover.Builder.Server.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpsRedirect(ApplicationSettings.HttpsPortKey)]
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }

        // GET api/values/5 
        [Authorize]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}