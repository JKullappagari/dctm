using Microsoft.EntityFrameworkCore;
﻿using System;
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
    public class OwnerDivAssignmentController : Controller
    {

        private readonly DCTrackContext _context;

        public OwnerDivAssignmentController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all owner-division assignments
        /// </summary>
        /// <returns></returns>
        // GET: api/ownerdivassignments
        [HttpGet]
        [ProducesResponseType(typeof(TblOwnerDivisionAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblOwnerDivisionAssignment>> Get()
        {
            List<TblOwnerDivisionAssignment> assignments = await (from g in _context.TblOwnerDivisionAssignment
                                            select g).AsNoTracking().ToListAsync();
            return assignments;
        }

        /// <summary>
        /// Gets owner-division assignment details based on identifier
        /// </summary>
        /// <param name="OwnerDivId"></param>
        /// <returns></returns>
        // GET api/ownerdivassignments/5
        [HttpGet("{OwnerDivId}")]
        [ProducesResponseType(typeof(TblOwnerDivisionAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblOwnerDivisionAssignment>> Get(int OwnerDivId)
        {
            List<TblOwnerDivisionAssignment> assignments = await (from g in _context.TblOwnerDivisionAssignment
                                                          where g.OwnerDivId == OwnerDivId
                                                          select g).AsNoTracking().ToListAsync();
            return assignments;
        }

        /// <summary>
        /// Gets owner-division assignment details which are modified after Last Updated datetime
        /// </summary>
        // GET api/ownerdivassignments/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblOwnerDivisionAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblOwnerDivisionAssignment>> Get(long LastUpdatedTime)
        {
            List<TblOwnerDivisionAssignment> ownerdivsions = await (from g in _context.TblOwnerDivisionAssignment
                                                            where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return ownerdivsions;
        }

        //// POST api/ownerdivassignments
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/ownerdivassignments/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/ownerdivassignments/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
