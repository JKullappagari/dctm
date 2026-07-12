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
    public class DivisionsController : Controller
    {
        private readonly DCTrackContext _context;

        public DivisionsController(DCTrackContext context)
        {
            _context = context;

        }


        /// <summary>
        /// Gets all divisions details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TblDivision), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblDivision>> Get()
        {
            List<TblDivision> divisions = await (from g in _context.TblDivision
                                                      select g).AsNoTracking().ToListAsync();
            return divisions;
        }

        /// <summary>
        /// Gets Air Flow Direction details based on identifier
        /// </summary>
        // GET api/AFDirections/5
        [HttpGet("{DivisionId}")]
        [ProducesResponseType(typeof(TblDivision), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblDivision>> Get(int DivisionId)
        {
            List<TblDivision> divisions = await (from g in _context.TblDivision
                                                     where g.DivisionId == DivisionId
                                             select g).AsNoTracking().ToListAsync();
            return divisions;
        }

        /// <summary>
        /// Gets Air Flow Direction details which are modified after Last Updated datetime
        /// </summary>
        // GET api/divisions/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblDivision), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblDivision>> Get(long LastUpdatedTime)
        {
            List<TblDivision> divisions = await (from g in _context.TblDivision
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return divisions;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="value"></param>
        //// PUT api/AFDirections/5
        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(TblDivision), 200)]
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
