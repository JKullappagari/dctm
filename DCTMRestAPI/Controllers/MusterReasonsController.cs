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
    public class MusterReasonsController : Controller
    {
        private readonly DCTrackContext _context;

        public MusterReasonsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all muster reasons 
        /// </summary>
        /// <returns></returns>
        // GET: api/reasons
        [HttpGet]
        [ProducesResponseType(typeof(TblMusterReason), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblMusterReason> Get()
        {
            List<TblMusterReason> reasons = (from g in _context.TblMusterReason
                                            select g).ToList();
            return reasons;
        }

        /// <summary>
        /// Gets muster reason based on identifier
        /// </summary>
        /// <param name="MusterReasonId"></param>
        /// <returns></returns>
        // GET api/reasons/5
        [HttpGet("{MusterReasonId}")]
        [ProducesResponseType(typeof(TblMusterReason), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblMusterReason> Get(int MusterReasonId)
        {
            List<TblMusterReason> reasons = (from g in _context.TblMusterReason
                                               where g.MusterReasonId == MusterReasonId
                                             select g).ToList();
            return reasons;
        }

        /// <summary>
        /// Gets muster reasons which are modified after Last Updated datetime
        /// </summary>
        // GET api/reasons/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblMusterReason), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblMusterReason> Get(long LastUpdatedTime)
        {
            List<TblMusterReason> reasons = (from g in _context.TblMusterReason
                                                 where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return reasons;
        }

        //// POST api/reasons
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/reasons/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
