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
    public class BladeModelsController : Controller
    {

        private readonly DCTrackContext _context;

        public BladeModelsController(DCTrackContext context)
        {
            _context = context;

        }
        /// <summary>
        /// Gets all blade models specifications
        /// </summary>
        /// <returns></returns>
        // GET: api/blademodels
        [HttpGet]
        [ProducesResponseType(typeof(TblBladeModelDetails), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblBladeModelDetails>> Get()
        {
            List<TblBladeModelDetails> bladeModels = await (from g in _context.TblBladeModelDetails
                                            select g).AsNoTracking().ToListAsync();
            return bladeModels;
        }

        /// <summary>
        /// Gets blade model specification based on identifier
        /// </summary>
        /// <param name="BladeModelId"></param>
        /// <returns></returns>
        // GET api/blademodels/5
        [HttpGet("{BladeModelId}")]
        [ProducesResponseType(typeof(TblBladeModelDetails), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblBladeModelDetails>> Get(int BladeModelId)
        {
            List<TblBladeModelDetails> bladeModels = await (from g in _context.TblBladeModelDetails
                                                    where g.BladeModelId == BladeModelId
                                                    select g).AsNoTracking().ToListAsync();
            return bladeModels;
        }

        /// <summary>
        /// Gets blade model specifications which are modified after Last Updated datetime
        /// </summary>
        // GET api/blademodels/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblBladeModelDetails), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblBladeModelDetails>> Get(long LastUpdatedTime)
        {
            List<TblBladeModelDetails> blademodels = await (from g in _context.TblBladeModelDetails
                                                      where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return blademodels;
        }

        //// POST api/blademodels
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/blademodels/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/blademodels/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
