using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyRESTService.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        // GET: api/person
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "personA", "personB" };
        }

		// GET api/person/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

		// POST api/person
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

		// PUT api/person/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

		// DELETE api/person/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
