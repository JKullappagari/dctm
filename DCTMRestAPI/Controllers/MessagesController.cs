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
    public class MessagesController : Controller
    {
        private readonly DCTrackContext _context;

        public MessagesController(DCTrackContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets all message codes
        /// </summary>
        /// <returns></returns>
        // GET: api/messages
        [HttpGet]
        [ProducesResponseType(typeof(TblMessage), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblMessage> Get()
        {
            List<TblMessage> messages = (from g in _context.TblMessage
                                            select g).ToList();
            return messages;
        }

        /// <summary>
        /// Gets message codes based on identifier
        /// </summary>
        /// <param name="MessageCodeId"></param>
        /// <returns></returns>
        // GET api/messages/5
        [HttpGet("{MessageCodeId}")]
        [ProducesResponseType(typeof(TblMessage), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblMessage> Get(int MessageCodeId)
        {
            List<TblMessage> messages = (from g in _context.TblMessage
                                          where g.MessageCodeId == MessageCodeId
                                         select g).ToList();
            return messages;
        }

        /// <summary>
        /// Gets message codes which are modified after Last Updated datetime
        /// </summary>
        // GET api/AFDirections/5
        [HttpGet("updated/{LastUpdatedTime}")]
        [ProducesResponseType(typeof(TblMessage), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IEnumerable<TblMessage> Get(long LastUpdatedTime)
        {
            List<TblMessage> codes = (from g in _context.TblMessage
                                            where g.LastUpdatedTime > LastUpdatedTime
                                                     select g).ToList();
            return codes;
        }

        //// POST api/messages
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}


        //// DELETE api/messages/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
