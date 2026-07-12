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
    public class TenantAssetAssignmentsController : Controller
    {
        private readonly DCTrackContext _context;

        public TenantAssetAssignmentsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all tenant-asset assignment details
        /// </summary>
        /// <returns></returns>
        // GET: api/Tenantassetassignments
        [HttpGet]
        [ProducesResponseType(typeof(TblTenantAssetAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTenantAssetAssignment>> Get()
        {
            List<TblTenantAssetAssignment> tenants = await (from g in _context.TblTenantAssetAssignment
                                            select g).AsNoTracking().ToListAsync();
            return tenants;
        }

        /// <summary>
        /// Gets tenant-asset assignment details based on identifier
        /// </summary>
        /// <param name="TenantAssetId"></param>
        /// <returns></returns>
        // GET api/tenantassetassignments/5
        [HttpGet("{TenantAssetId}")]
        [ProducesResponseType(typeof(TblTenantAssetAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTenantAssetAssignment>> Get(int TenantAssetId)
        {
            List<TblTenantAssetAssignment> buSite = await (from g in _context.TblTenantAssetAssignment
                                                   where g.TenantAssetId == TenantAssetId
                                                           select g).AsNoTracking().ToListAsync();
            return buSite;
        }

        /// <summary>
        /// Gets tenant-asset assignment details which are modified after Last Updated datetime
        /// </summary>
        // GET api/tenantassetassignments/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblTenantAssetAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<TblTenantAssetAssignment>> Get(long LastUpdatedTime)
        {
            List<TblTenantAssetAssignment> busite = await (from g in _context.TblTenantAssetAssignment
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).AsNoTracking().ToListAsync();
            return busite;
        }

       
    }
}
