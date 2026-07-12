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
    public class CheckDeleteController : Controller
    {
        private readonly DCTrackContext _context;

        public CheckDeleteController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all check delete specifications
        /// </summary>
        /// <returns></returns>
        // GET: api/checkdelete
        [ProducesResponseType(typeof(TblCheckDelete), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpGet]
        public async Task<IEnumerable<TblCheckDelete>> Get()
        {
            List<TblCheckDelete> checkDelete = await (from g in _context.TblCheckDelete
                                            select g).AsNoTracking().ToListAsync();
            return checkDelete;
        }

        /// <summary>
        /// Gets check delete specifications based on identifier
        /// </summary>
        /// <param name="ColumnTableId"></param>
        /// <returns></returns>
        // GET api/checkdelete/5
        [HttpGet("{ColumnTableId}")]
        [ProducesResponseType(typeof(TblCheckDelete), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblCheckDelete>> Get(int ColumnTableId)
        {
            List<TblCheckDelete> checkDelete = await (from g in _context.TblCheckDelete
                                              where g.ColumnTableId == ColumnTableId
                                                select g).AsNoTracking().ToListAsync();
            return checkDelete;
        }

        /// <summary>
        /// Gets check delete specifications which are modified after Last Updated datetime
        /// </summary>
        // GET api/checkdelete/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblCheckDelete), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblCheckDelete>> Get(long LastUpdatedTime)
        {
            List<TblCheckDelete> checkdelete = await (from g in _context.TblCheckDelete
                                                where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return checkdelete;
        }

        //// POST api/checkdelete
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/checkdelete/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
