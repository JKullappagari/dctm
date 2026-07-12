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
    public class AppCriticalitiesController : Controller
    {
        private readonly DCTrackContext _context;

        public AppCriticalitiesController(DCTrackContext context)
        {
            _context = context;

        }
        /// <summary>
        /// Gets all application criticality definitions 
        /// </summary>
        /// <returns></returns>
        // GET: api/AppCriticalities
        [HttpGet]
        [ProducesResponseType(typeof(TblApplCriticality), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblApplCriticality>> Get()
        {
            List<TblApplCriticality> appCritics = await (from g in _context.TblApplCriticality
                                                    select g).AsNoTracking().ToListAsync();
            return appCritics;
        }

        /// <summary>
        /// Gets application criticality details based on identifier
        /// </summary>
        /// <param name="ApplCriticalityId"></param>
        /// <returns></returns>
        // GET api/AppCriticalities/5
        [HttpGet("{ApplCriticalityId}")]
        [ProducesResponseType(typeof(TblApplCriticality), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblApplCriticality>> Get(int ApplCriticalityId)
        {
            List<TblApplCriticality> appCritics = await (from g in _context.TblApplCriticality
                                                   where g.ApplCriticalityId == ApplCriticalityId
                                                   select g).AsNoTracking().ToListAsync();
            return appCritics;
        }

        /// <summary>
        /// Gets application criticality definitions which are modified after Last Updated datetime
        /// </summary>
        // GET api/AppCriticalities/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblApplCriticality), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblApplCriticality>> Get(long LastUpdatedTime)
        {
            List<TblApplCriticality> criticalities = await (from g in _context.TblApplCriticality
                                                    where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return criticalities;
        }


        //// POST api/AppCriticalities
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/AppCriticalities/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/AppCriticalities/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
