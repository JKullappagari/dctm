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
    public class BuSiteAssignmentController : Controller
    {
        private readonly DCTrackContext _context;

        public BuSiteAssignmentController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all bu-site assignment details
        /// </summary>
        /// <returns></returns>
        // GET: api/busiteassignments
        [HttpGet]
        [ProducesResponseType(typeof(TblBusiteAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblBusiteAssignment> Get()
        {
            List<TblBusiteAssignment> buSite = (from g in _context.TblBusiteAssignment
                                            select g).ToList();
            return buSite;
        }

        /// <summary>
        /// Gets bu-site assignment based on identifier
        /// </summary>
        /// <param name="BusiteAssignmentId"></param>
        /// <returns></returns>
        // GET api/busiteassignments/5
        [HttpGet("{BusiteAssignmentId}")]
        [ProducesResponseType(typeof(TblBusiteAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblBusiteAssignment> Get(int BusiteAssignmentId)
        {
            List<TblBusiteAssignment> buSite = (from g in _context.TblBusiteAssignment
                                                   where g.BusiteAssignmentId == BusiteAssignmentId
                                                select g).ToList();
            return buSite;
        }

        /// <summary>
        /// Gets bu-site assignment details which are modified after Last Updated datetime
        /// </summary>
        // GET api/busiteassignments/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblBusiteAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblBusiteAssignment> Get(long LastUpdatedTime)
        {
            List<TblBusiteAssignment> busite = (from g in _context.TblBusiteAssignment
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return busite;
        }

        //// POST api/busiteassignments
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}


        //// DELETE api/busiteassignments/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
