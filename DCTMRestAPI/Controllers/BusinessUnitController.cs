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
    public class BusinessUnitController : Controller
    {

        private readonly DCTrackContext _context;

        public BusinessUnitController(DCTrackContext context)
        {
            _context = context;

        }
        /// <summary>
        /// Gets all business units
        /// </summary>
        /// <returns></returns>
        // GET: api/businessunits
        [HttpGet]
        [ProducesResponseType(typeof(TblBusinessUnit), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblBusinessUnit> Get()
        {
            List<TblBusinessUnit> bu = (from g in _context.TblBusinessUnit
                                               select g).ToList();
            return bu;
        }
        /// <summary>
        /// Gets business unit details based on identifier
        /// </summary>
        /// <param name="BusinessUnitId"></param>
        /// <returns></returns>
        // GET api/businessunits/5
        [HttpGet("{BusinessUnitId}")]
        [ProducesResponseType(typeof(TblBusinessUnit), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblBusinessUnit> Get(int BusinessUnitId)
        {
            List<TblBusinessUnit> bu = (from g in _context.TblBusinessUnit
                                               where g.BusinessUnitId == BusinessUnitId
                                        select g).ToList();
            return bu;
        }

        /// <summary>
        /// Gets business unit details which are modified after Last Updated datetime
        /// </summary>
        // GET api/businessunits/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblBusinessUnit), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblBusinessUnit> Get(long LastUpdatedTime)
        {
            List<TblBusinessUnit> bus = (from g in _context.TblBusinessUnit
                                                 where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return bus;
        }

        //// POST api/businessunits
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/businessunits/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
