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
    public class StatusMasterController : Controller
    {
        private readonly DCTrackContext _context;

        public StatusMasterController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all status master definitions
        /// </summary>
        /// <returns></returns>
        // GET: api/statuses
        [HttpGet]
        [ProducesResponseType(typeof(TblStatusMaster), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblStatusMaster> Get()
        {
            List<TblStatusMaster> statuses = (from g in _context.TblStatusMaster
                                            select g).ToList();
            return statuses;
        }

        /// <summary>
        /// Gets status master definitions based on identifier
        /// </summary>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        // GET api/statuses/5
        [HttpGet("{StatusId}")]
        [ProducesResponseType(typeof(TblStatusMaster), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblStatusMaster> Get(int StatusId)
        {
            List<TblStatusMaster> statuses = (from g in _context.TblStatusMaster
                                               where g.StatusId == StatusId
                                              select g).ToList();
            return statuses;
        }

        /// <summary>
        /// Gets status master definitions which are modified after Last Updated datetime
        /// </summary>
        // GET api/statuses/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblStatusMaster), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblStatusMaster> Get(long LastUpdatedTime)
        {
            List<TblStatusMaster> statuses = (from g in _context.TblStatusMaster
                                                 where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return statuses;
        }

        //// POST api/statuses
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/statuses/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/statuses/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
