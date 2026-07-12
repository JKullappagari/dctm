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
    public class InputConnectorTypesController : Controller
    {

        private readonly DCTrackContext _context;

        public InputConnectorTypesController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all input connector types
        /// </summary>
        /// <returns></returns>
        // GET: api/inputconnectortypes
        [HttpGet]
        [ProducesResponseType(typeof(TblInputConnectorType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblInputConnectorType>> Get()
        {
            List<TblInputConnectorType> connectorTypes = await (from g in _context.TblInputConnectorType
                                            select g).AsNoTracking().ToListAsync();
            return connectorTypes;
        }

        /// <summary>
        /// Gets input connector type based on identifier
        /// </summary>
        /// <param name="InputConnectorTypeId"></param>
        /// <returns></returns>
        // GET api/inputconnectortypes/5
        [HttpGet("{InputConnectorTypeId}")]
        [ProducesResponseType(typeof(TblInputConnectorType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblInputConnectorType>> Get(int InputConnectorTypeId)
        {
            List<TblInputConnectorType> connectorTypes = await (from g in _context.TblInputConnectorType
                                                     where g.InputConnectorTypeId == InputConnectorTypeId
                                                     select g).AsNoTracking().ToListAsync();
            return connectorTypes;
        }

        /// <summary>
        /// Gets input connector types which are modified after Last Updated datetime
        /// </summary>
        // GET api/inputconnectortypes/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblInputConnectorType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblInputConnectorType>> Get(long LastUpdatedTime)
        {
            List<TblInputConnectorType> inputconnectors = await (from g in _context.TblInputConnectorType
                                                       where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return inputconnectors;
        }

        //// POST api/inputconnectortypes
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/inputconnectortypes/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
