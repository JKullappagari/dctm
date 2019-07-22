using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCTMRestAPI.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class OwnersController : Controller
    {
        private readonly DCTrackContext _context;

        public OwnersController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all owner/custodian details
        /// </summary>
        /// <returns></returns>
        // GET: api/owners
        [HttpGet]
        [ProducesResponseType(typeof(TblOwner), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblOwner> Get()
        {
            List<TblOwner> owners = (from g in _context.TblOwner
                                     select g).ToList();
            return owners;
        }

        /// <summary>
        /// Gets owner/custodian details based on identifier
        /// </summary>
        /// <param name="OwnerId"></param>
        /// <returns></returns>
        // GET api/owners/5
        [HttpGet("{OwnerId}")]
        [ProducesResponseType(typeof(TblOwner), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblOwner> Get(int OwnerId)
        {
            List<TblOwner> owners = (from g in _context.TblOwner
                                        where g.OwnerId == OwnerId
                                        select g).ToList();
            return owners;
        }

        /// <summary>
        /// Gets owner/custodian which are modified after Last Updated datetime
        /// </summary>
        // GET api/owners/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblOwner), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblOwner> Get(long LastUpdatedTime)
        {
            List<TblOwner> owners = (from g in _context.TblOwner
                                          where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return owners;
        }

        //// POST api/owners
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/owners/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/owners/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
