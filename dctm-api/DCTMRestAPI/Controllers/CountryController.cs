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
    public class CountriesController : Controller
    {
        private readonly DCTrackContext _context;

        public CountriesController(DCTrackContext context)
        {
            _context = context;

        }


        /// <summary>
        /// Gets all country details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TblCountry), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblCountry>> Get()
        {
            List<TblCountry> afDirections = await (from g in _context.TblCountry
                                                      select g).AsNoTracking().ToListAsync();
            return afDirections;
        }

        /// <summary>
        /// Gets country details based on identifier
        /// </summary>
        // GET api/countries/5
        [HttpGet("{CountryId}")]
        [ProducesResponseType(typeof(TblCountry), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblCountry>> Get(int CountryId)
        {
            List<TblCountry> countries = await (from g in _context.TblCountry
                                                     where g.CountryId == CountryId
                                            select g).AsNoTracking().ToListAsync();
            return countries;
        }

        /// <summary>
        /// Gets countries details which are modified after Last Updated datetime
        /// </summary>
        // GET api/countries/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblCountry), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblCountry>> Get(long LastUpdatedTime)
        {
            List<TblCountry> countries = await (from g in _context.TblCountry
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return countries;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="value"></param>
        //// PUT api/AFDirections/5
        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(TblCountry), 200)]
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
