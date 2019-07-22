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
    public class UOMController : Controller
    {
        private readonly DCTrackContext _context;

        public UOMController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all unit of measurement details
        /// </summary>
        /// <returns></returns>
        // GET: api/uom
        [HttpGet]
        [ProducesResponseType(typeof(TblUom), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblUom> Get()
        {
            List<TblUom> uom = (from g in _context.TblUom
                                            select g).ToList();
            return uom;
        }

        /// <summary>
        /// Gets uom details which are modified after Last Updated datetime
        /// </summary>
        // GET api/uom/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblUom), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblUom> Get(long LastUpdatedTime)
        {
            List<TblUom> uom = (from g in _context.TblUom
                                        where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return uom;
        }


        //// POST api/uom
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/uom/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/uom/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
