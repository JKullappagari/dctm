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
    public class MountTypesController : Controller
    {
        private readonly DCTrackContext _context;

        public MountTypesController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all mount types 
        /// </summary>
        /// <returns></returns>
        // GET: api/mounttypes
        [HttpGet]
        [ProducesResponseType(typeof(TblMountType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblMountType> Get()
        {
            List<TblMountType> mountTypes = (from g in _context.TblMountType
                                            select g).ToList();
            return mountTypes;
        }

        /// <summary>
        /// Gets mount type based on identifier
        /// </summary>
        /// <param name="Mounttypeid"></param>
        /// <returns></returns>
        // GET api/mounttypes/5
        [HttpGet("{Mounttypeid}")]
        [ProducesResponseType(typeof(TblMountType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblMountType> Get(int Mounttypeid)
        {
            List<TblMountType> mountTypes = (from g in _context.TblMountType
                                            where g.Mounttypeid == Mounttypeid
                                             select g).ToList();
            return mountTypes;
        }

        /// <summary>
        /// Getmount types which are modified after Last Updated datetime
        /// </summary>
        // GET api/mounttypes/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblMountType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblMountType> Get(long LastUpdatedTime)
        {
            List<TblMountType> types = (from g in _context.TblMountType
                                              where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return types;
        }

        //// POST api/mounttypes
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// DELETE api/mounttypes/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
