using System;
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
    public class TenantApplicationAssignmentsController : Controller
    {
        private readonly DCTrackContext _context;

        public TenantApplicationAssignmentsController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all tenant-application assignment details
        /// </summary>
        /// <returns></returns>
        // GET: api/Tenantapplicationassignments
        [HttpGet]
        [ProducesResponseType(typeof(TblTenantApplicationAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblTenantApplicationAssignment> Get()
        {
            List<TblTenantApplicationAssignment> tenants = (from g in _context.TblTenantApplicationAssignment
                                            select g).ToList();
            return tenants;
        }

        /// <summary>
        /// Gets tenant-application assignment details based on identifier
        /// </summary>
        /// <param name="TenantApplicationId"></param>
        /// <returns></returns>
        // GET api/tenantapplicationassignments/5
        [HttpGet("{TenantApplicationId}")]
        [ProducesResponseType(typeof(TblTenantApplicationAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblTenantApplicationAssignment> Get(int TenantApplicationId)
        {
            List<TblTenantApplicationAssignment> buSite = (from g in _context.TblTenantApplicationAssignment
                                                   where g.TenantApplicationId == TenantApplicationId
                                                           select g).ToList();
            return buSite;
        }

        /// <summary>
        /// Gets tenant-application assignment details which are modified after Last Updated datetime
        /// </summary>
        // GET api/tenantapplicationassignments/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblTenantApplicationAssignment), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblTenantApplicationAssignment> Get(long LastUpdatedTime)
        {
            List<TblTenantApplicationAssignment> busite = (from g in _context.TblTenantApplicationAssignment
                                                     where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return busite;
        }

       
    }
}
