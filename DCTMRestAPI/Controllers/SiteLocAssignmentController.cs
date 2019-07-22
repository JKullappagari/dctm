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
    public class SiteLocAssignmentController : Controller
    {
        private readonly DCTrackContext _context;

        public SiteLocAssignmentController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all site-location assignment details
        /// </summary>
        /// <returns></returns>
        // GET: api/sitelocassignments
        [HttpGet]
        [ProducesResponseType(typeof(TblSiteLocationAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblSiteLocationAssignment> Get()
        {
            List<TblSiteLocationAssignment> assignments = (from g in _context.TblSiteLocationAssignment
                                            select g).ToList();
            return assignments;
        }

        /// <summary>
        /// Gets site-location assignment details based on identifier
        /// </summary>
        /// <param name="SiteLocationAssignmentId"></param>
        /// <returns></returns>
        // GET api/sitelocassignments/5
        [HttpGet("{SiteLocationAssignmentId}")]
        [ProducesResponseType(typeof(TblSiteLocationAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblSiteLocationAssignment> Get(int SiteLocationAssignmentId)
        {
            List<TblSiteLocationAssignment> assignments = (from g in _context.TblSiteLocationAssignment
                                                         where g.SiteLocationAssignmentId == SiteLocationAssignmentId
                                                           select g).ToList();
            return assignments;
        }

        /// <summary>
        /// Gets site-location assignment details which are modified after Last Updated datetime
        /// </summary>
        // GET api/sitelocassignments/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblSiteLocationAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblSiteLocationAssignment> Get(long LastUpdatedTime)
        {
            List<TblSiteLocationAssignment> siteloc = (from g in _context.TblSiteLocationAssignment
                                                           where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return siteloc;
        }

        //// POST api/sitelocassignments
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/sitelocassignments/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/sitelocassignments/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
