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
    public class TenantLocationAssignmentsController : Controller
    {
        private readonly DCTrackContext _context;

        public TenantLocationAssignmentsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all tenant-location assignment details
        /// </summary>
        /// <returns></returns>
        // GET: api/Tenantlocationassignments
        [HttpGet]
        [ProducesResponseType(typeof(TblTenantLocationAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTenantLocationAssignment>> Get()
        {
            List<TblTenantLocationAssignment> tenants = await (from g in _context.TblTenantLocationAssignment
                                            select g).AsNoTracking().ToListAsync();
            return tenants;
        }

        /// <summary>
        /// Gets tenant-location assignment details based on identifier
        /// </summary>
        /// <param name="TenantLocationId"></param>
        /// <returns></returns>
        // GET api/tenantlocationassignments/5
        [HttpGet("{TenantLocationId}")]
        [ProducesResponseType(typeof(TblTenantLocationAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTenantLocationAssignment>> Get(int TenantLocationId)
        {
            List<TblTenantLocationAssignment> buSite = await (from g in _context.TblTenantLocationAssignment
                                                   where g.TenantLocationId == TenantLocationId
                                                           select g).AsNoTracking().ToListAsync();
            return buSite;
        }

        /// <summary>
        /// Gets tenant-location assignment details which are modified after Last Updated datetime
        /// </summary>
        // GET api/tenantlocationassignments/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblTenantLocationAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTenantLocationAssignment>> Get(long LastUpdatedTime)
        {
            List<TblTenantLocationAssignment> busite = await (from g in _context.TblTenantLocationAssignment
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return busite;
        }

       
    }
}
