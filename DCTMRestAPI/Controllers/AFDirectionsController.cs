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
    public class AFDirectionsController : Controller
    {
        private readonly DCTrackContext _context;

        public AFDirectionsController(DCTrackContext context)
        {
            _context = context;

        }


        /// <summary>
        /// Gets all air flow directions details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TblAirFlowDirection), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAirFlowDirection> Get()
        {
            List<TblAirFlowDirection> afDirections = (from g in _context.TblAirFlowDirection
                                                      select g).ToList();
            return afDirections;
        }

        /// <summary>
        /// Gets air flow directions details based on identifier
        /// </summary>
        // GET api/AFDirections/5
        [HttpGet("{ID}")]
        [ProducesResponseType(typeof(TblAirFlowDirection), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAirFlowDirection> Get(int ID)
        {
            List<TblAirFlowDirection> afDirection = (from g in _context.TblAirFlowDirection
                                                     where g.Id == ID
                                                     select g).ToList();
            return afDirection;
        }

        /// <summary>
        /// Gets air flow directions details which are modified after Last Updated datetime
        /// </summary>
        // GET api/AFDirections/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblAirFlowDirection), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblAirFlowDirection> Get(long LastUpdatedTime)
        {
            List<TblAirFlowDirection> afDirection = (from g in _context.TblAirFlowDirection
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return afDirection;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="value"></param>
        //// PUT api/AFDirections/5
        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(TblAirFlowDirection), 200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(401)]
        //public void Put(int id, [FromBody]string value)
        //{

        //}


        //// POST api/AFDirections
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/AFDirections/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
