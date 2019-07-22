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
    public class CitiesController : Controller
    {
        private readonly DCTrackContext _context;

        public CitiesController(DCTrackContext context)
        {
            _context = context;

        }


        /// <summary>
        /// Gets all city details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TblCity), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblCity> Get()
        {
            List<TblCity> afDirections = (from g in _context.TblCity
                                                      select g).ToList();
            return afDirections;
        }

        /// <summary>
        /// Gets Air Flow Direction details based on identifier
        /// </summary>
        // GET api/city/5
        [HttpGet("{CityId}")]
        [ProducesResponseType(typeof(TblCity), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblCity> Get(int CityId)
        {
            List<TblCity> cities = (from g in _context.TblCity
                                                     where g.CityId == CityId
                                         select g).ToList();
            return cities;
        }

        /// <summary>
        /// Gets Air Flow Direction details which are modified after Last Updated datetime
        /// </summary>
        // GET api/AFDirections/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblCity), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblCity> Get(long LastUpdatedTime)
        {
            List<TblCity> afDirection = (from g in _context.TblCity
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
        //[ProducesResponseType(typeof(TblCity), 200)]
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
