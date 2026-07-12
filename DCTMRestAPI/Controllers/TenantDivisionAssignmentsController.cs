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
    public class TenantDivisionAssignmentsController : Controller
    {
        private readonly DCTrackContext _context;

        public TenantDivisionAssignmentsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all tenant-division assignment details
        /// </summary>
        /// <returns></returns>
        // GET: api/Tenantdivisionassignments
        [HttpGet]
        [ProducesResponseType(typeof(TblTenantDivisionAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTenantDivisionAssignment>> Get()
        {
            List<TblTenantDivisionAssignment> tenants = await (from g in _context.TblTenantDivisionAssignment
                                            select g).AsNoTracking().ToListAsync();
            return tenants;
        }

        /// <summary>
        /// Gets tenant-division assignment details based on identifier
        /// </summary>
        /// <param name="TenantDivisionId"></param>
        /// <returns></returns>
        // GET api/tenantdivisionassignments/5
        [HttpGet("{TenantDivisionId}")]
        [ProducesResponseType(typeof(TblTenantDivisionAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTenantDivisionAssignment>> Get(int TenantDivisionId)
        {
            List<TblTenantDivisionAssignment> buSite = await (from g in _context.TblTenantDivisionAssignment
                                                   where g.TenantDivisionId == TenantDivisionId
                                                           select g).AsNoTracking().ToListAsync();
            return buSite;
        }

        /// <summary>
        /// Gets tenant-division assignment details which are modified after Last Updated datetime
        /// </summary>
        // GET api/tenantdivisionassignments/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblTenantDivisionAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTenantDivisionAssignment>> Get(long LastUpdatedTime)
        {
            List<TblTenantDivisionAssignment> busite = await (from g in _context.TblTenantDivisionAssignment
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return busite;
        }

       
    }
}
