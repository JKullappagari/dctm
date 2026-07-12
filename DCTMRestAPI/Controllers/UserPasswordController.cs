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
    public class UserPasswordController : Controller
    {
        private readonly DCTrackContext _context;

        public UserPasswordController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all user password details
        /// </summary>
        /// <returns></returns>
        // GET: api/userpassword
        [HttpGet]
        [ProducesResponseType(typeof(TblUserPassword), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblUserPassword>> Get()
        {
            List<TblUserPassword> passwords = await (from g in _context.TblUserPassword
                                            select g).AsNoTracking().ToListAsync();
            return passwords;
        }

        /// <summary>
        /// Gets user password details based on identifier
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        // GET api/userpassword/5
        [HttpGet("{UserId}")]
        [ProducesResponseType(typeof(TblUserPassword), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblUserPassword>> Get(int UserId)
        {
            List<TblUserPassword> passwords = await (from g in _context.TblUserPassword
                                               where g.UserId == UserId
                                               select g).AsNoTracking().ToListAsync();
            return passwords;
        }

        /// <summary>
        /// Gets user password details which are modified after Last Updated datetime
        /// </summary>
        // GET api/AFDirections/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblUserPassword), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblUserPassword>> Get(long LastUpdatedTime)
        {
            List<TblUserPassword> passwords = await (from g in _context.TblUserPassword
                                                 where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return passwords;
        }

        //// POST api/userpassword
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/userpassword/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/userpassword/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
