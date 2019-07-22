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
    public class EntityTypesController : Controller
    {

        private readonly DCTrackContext _context;

        public EntityTypesController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all entity types 
        /// </summary>
        /// <returns></returns>
        // GET: api/entitytypes
        [HttpGet]
        [ProducesResponseType(typeof(TblEntityType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblEntityType> Get()
        {
            List<TblEntityType> entityTypes = (from g in _context.TblEntityType
                                             select g).ToList();
            return entityTypes;
        }

        /// <summary>
        /// Gets entity type based on identifier
        /// </summary>
        /// <param name="EntityTypeId"></param>
        /// <returns></returns>
        // GET api/entitytypes/5
        [HttpGet("{EntityTypeId}")]
        [ProducesResponseType(typeof(TblEntityType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblEntityType> Get(int EntityTypeId)
        {
            List<TblEntityType> entityTypes = (from g in _context.TblEntityType
                                             where g.EntityTypeId == EntityTypeId
                                               select g).ToList();
            return entityTypes;
        }

        /// <summary>
        /// Gets entity types which are modified after Last Updated datetime
        /// </summary>
        // GET api/entitytypes/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblEntityType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblEntityType> Get(long LastUpdatedTime)
        {
            List<TblEntityType> entitytypes = (from g in _context.TblEntityType
                                               where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return entitytypes;
        }


        //// POST api/entitytypes
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/entitytypes/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
