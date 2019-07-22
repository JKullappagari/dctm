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
    public class ManufacturersController : Controller
    {

        private readonly DCTrackContext _context;

        public ManufacturersController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <returns></returns>
        // GET: api/manufacturers
        [HttpGet]
        [ProducesResponseType(typeof(TblManufacturer), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblManufacturer> Get()
        {
            List<TblManufacturer> mfgs = (from g in _context.TblManufacturer
                                                 select g).ToList();
            return mfgs;
        }

        /// <summary>
        /// Gets manufacturers based on identifier
        /// </summary>
        /// <param name="MfgId"></param>
        /// <returns></returns>
        // GET api/manufacturers/5
        [HttpGet("{MfgId}")]
        [ProducesResponseType(typeof(TblManufacturer), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblManufacturer> Get(int MfgId)
        {
            List<TblManufacturer> mfgs = (from g in _context.TblManufacturer
                                          where g.MfgId == MfgId
                                          select g).ToList();
            return mfgs;
        }

        /// <summary>
        /// Gets manufacturers which are modified after Last Updated datetime
        /// </summary>
        // GET api/manufacturers/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblManufacturer), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblManufacturer> Get(long LastUpdatedTime)
        {
            List<TblManufacturer> mfgs = (from g in _context.TblManufacturer
                                                 where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return mfgs;
        }

        //// POST api/manufacturers
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}



        //// DELETE api/manufacturers/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
