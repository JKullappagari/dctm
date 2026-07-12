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
    public class OutputConnectorTypesController : Controller
    {

        private readonly DCTrackContext _context;

        public OutputConnectorTypesController(DCTrackContext context)
        {
            _context = context;

        }
        /// <summary>
        /// Gets all output connector types
        /// </summary>
        /// <returns></returns>
        // GET: api/outputconnectortypes
        [HttpGet]
        [ProducesResponseType(typeof(TblOutputConnectorType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblOutputConnectorType>> Get()
        {
            List<TblOutputConnectorType> connectorTypes = await (from g in _context.TblOutputConnectorType
                                                           select g).AsNoTracking().ToListAsync();
            return connectorTypes;
        }

        /// <summary>
        /// Gets output connector type based on identifier
        /// </summary>
        /// <param name="OutputConnectorTypeId"></param>
        /// <returns></returns>
        // GET api/outputconnectortypes/5
        [HttpGet("{OutputConnectorTypeId}")]
        [ProducesResponseType(typeof(TblOutputConnectorType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblOutputConnectorType>> Get(int OutputConnectorTypeId)
        {
            List<TblOutputConnectorType> connectorTypes = await (from g in _context.TblOutputConnectorType
                                                      where g.OutputConnectorTypeId == OutputConnectorTypeId
                                                           select g).AsNoTracking().ToListAsync();
            return connectorTypes;
        }

        /// <summary>
        /// Gets output connector types which are modified after Last Updated datetime
        /// </summary>
        // GET api/outputconnectortypes/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblOutputConnectorType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblOutputConnectorType>> Get(long LastUpdatedTime)
        {
            List<TblOutputConnectorType> connectortypes = await (from g in _context.TblOutputConnectorType
                                                        where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return connectortypes;
        }

        //// POST api/outputconnectortypes
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/outputconnectortypes/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/outputconnectortypes/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
