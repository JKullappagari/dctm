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
    public class LocationTypesController : Controller
    {

        private readonly DCTrackContext _context;

        public LocationTypesController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all location types 
        /// </summary>
        /// <returns></returns>
        // GET: api/locationtypes
        [HttpGet]
        [ProducesResponseType(typeof(TblLocationType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblLocationType> Get()
        {
            List<TblLocationType> locationtypes = (from g in _context.TblLocationType
                                                   select g).ToList();
            return locationtypes;
        }

        /// <summary>
        /// Gets location types based on identifier
        /// </summary>
        /// <param name="LocationTypeId"></param>
        /// <returns></returns>
        // GET api/locationtypes/5
        [HttpGet("{LocationTypeId}")]
        [ProducesResponseType(typeof(TblLocationType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblLocationType> Get(int LocationTypeId)
        {
            List<TblLocationType> locationtypes = (from g in _context.TblLocationType
                                                   where g.LocationTypeId == LocationTypeId
                                                   select g).ToList();
            return locationtypes;
        }


        /// <summary>
        /// Gets location types which are modified after Last Updated datetime
        /// </summary>
        // GET api/locationtypes/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblLocationType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblLocationType> Get(long LastUpdatedTime)
        {
            List<TblLocationType> locationtypes = (from g in _context.TblLocationType
                                                 where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return locationtypes;
        }

        //// POST api/locationtypes
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/locationtypes/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
