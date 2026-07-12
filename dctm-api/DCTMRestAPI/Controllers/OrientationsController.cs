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
    public class OrientationsController : Controller
    {
        private readonly DCTrackContext _context;

        public OrientationsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all orientation details
        /// </summary>
        /// <returns></returns>
        // GET: api/orientations
        [HttpGet]
        [ProducesResponseType(typeof(TblOrientation), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblOrientation>> Get()
        {
            List<TblOrientation> orientations = await (from g in _context.TblOrientation
                                              select g).AsNoTracking().ToListAsync();
            return orientations;
        }

        /// <summary>
        /// Gets orientation details based on identifier
        /// </summary>
        /// <param name="OrientationId"></param>
        /// <returns></returns>
        // GET api/orientations/5
        [HttpGet("{OrientationId}")]
        [ProducesResponseType(typeof(TblOrientation), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblOrientation>> Get(int OrientationId)
        {
            List<TblOrientation> orientations = await (from g in _context.TblOrientation
                                              where g.OrientationId == OrientationId
                                                 select g).AsNoTracking().ToListAsync();
            return orientations;
        }

        /// <summary>
        /// Gets orientation details which are modified after Last Updated datetime
        /// </summary>
        // GET api/orientations/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblOrientation), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblOrientation>> Get(long LastUpdatedTime)
        {
            List<TblOrientation> orientations = await (from g in _context.TblOrientation
                                                where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return orientations;
        }

        //// POST api/orientations
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/orientations/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
