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
    public class TransactionTypesController : Controller
    {
        private readonly DCTrackContext _context;

        public TransactionTypesController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all transaction types
        /// </summary>
        /// <returns></returns>
        // GET: api/transactiontypes
        [HttpGet]
        [ProducesResponseType(typeof(TblTransactionTypes), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTransactionTypes>> Get()
        {
            List<TblTransactionTypes> types = await (from g in _context.TblTransactionTypes
                                            select g).AsNoTracking().ToListAsync();
            return types;
        }

        /// <summary>
        /// Gets transaction types based on identifier
        /// </summary>
        /// <param name="TransTypeId"></param>
        /// <returns></returns>
        // GET api/transactiontypes/5
        [HttpGet("{TransTypeId}")]
        [ProducesResponseType(typeof(TblTransactionTypes), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTransactionTypes>> Get(int TransTypeId)
        {
            List<TblTransactionTypes> types = await (from g in _context.TblTransactionTypes
                                                   where g.TransTypeId == TransTypeId
                                               select g).AsNoTracking().ToListAsync();
            return types;
        }

        /// <summary>
        /// Gets transaction types which are modified after Last Updated datetime
        /// </summary>
        // GET api/transactiontypes/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblTransactionTypes), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTransactionTypes>> Get(long LastUpdatedTime)
        {
            List<TblTransactionTypes> types = await (from g in _context.TblTransactionTypes
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return types;
        }

        //// POST api/transactiontypes
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/transactiontypes/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/transactiontypes/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
