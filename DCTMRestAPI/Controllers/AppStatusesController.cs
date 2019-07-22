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
    public class AppStatusesController : Controller
    {
        private readonly DCTrackContext _context;

        public AppStatusesController(DCTrackContext context)
        {
            _context = context;

        }
        /// <summary>
        /// Gets all application status definitions
        /// </summary>
        /// <returns></returns>
        // GET: api/AppStatuses
        [HttpGet]
        [ProducesResponseType(typeof(TblAppStatus), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAppStatus> Get()
        {
            List<TblAppStatus> appStatuses = (from g in _context.TblAppStatus
                                               select g).ToList();
            return appStatuses;
        }

        /// <summary>
        /// Gets application status definition based on identifier
        /// </summary>
        /// <param name="AppStatusID"></param>
        /// <returns></returns>
        // GET api/AppStatuses/5
        [HttpGet("{AppStatusID}")]
        [ProducesResponseType(typeof(TblAppStatus), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAppStatus> Get(int AppStatusID)
        {
            List<TblAppStatus> appStatus = (from g in _context.TblAppStatus
                                          where g.AppStatusId == AppStatusID
                                            select g).ToList();
            return appStatus;
        }

        /// <summary>
        /// Gets application statuses which are modified after Last Updated datetime
        /// </summary>
        // GET api/AppStatuses/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblAppStatus), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAppStatus> Get(long LastUpdatedTime)
        {
            List<TblAppStatus> statuses = (from g in _context.TblAppStatus
                                              where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return statuses;
        }

        //// POST api/AppStatuses
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/AppStatuses/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
