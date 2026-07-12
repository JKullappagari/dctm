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
    public class ApplTypesController : Controller
    {
        private readonly DCTrackContext _context;

        public ApplTypesController(DCTrackContext context)
        {
            _context = context;

        }


        /// <summary>
        /// Gets all application types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TblApplType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblApplType>> Get()
        {
            List<TblApplType> types = await (from g in _context.TblApplType
                                              select g).AsNoTracking().ToListAsync();
            return types;
        }

        /// <summary>
        /// Gets application types based on identifier
        /// </summary>
        // GET api/appltypes/5
        [HttpGet("{ApplTypeId}")]
        [ProducesResponseType(typeof(TblApplType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblApplType>> Get(int ApplTypeId)
        {
            List<TblApplType> types = await (from g in _context.TblApplType
                                                     where g.ApplTypeId == ApplTypeId
                                             select g).AsNoTracking().ToListAsync();
            return types;
        }

        /// <summary>
        /// Gets application types which are modified after Last Updated datetime
        /// </summary>
        // GET api/appltypes/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblApplType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblApplType>> Get(long LastUpdatedTime)
        {
            List<TblApplType> types = await (from g in _context.TblApplType
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return types;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="value"></param>
        //// PUT api/AFDirections/5
        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(TblApplType), 200)]
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
