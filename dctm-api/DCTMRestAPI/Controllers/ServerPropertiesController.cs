using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using DCTMRestAPI.Models.Custom;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCTMRestAPI.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class ServerPropertiesController : Controller
    {
        private readonly DCTrackContext _context;

        public ServerPropertiesController(DCTrackContext context)
        {
            _context = context;

        }


        /// <summary>
        /// Gets current server date time in gmt format
        /// </summary>
        /// <returns></returns>
        [HttpGet("currentdatetime/utc")]
        [ProducesResponseType(typeof(DateTime), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public DateTime Get()
        {
            List<TblServerProperties> serverPorp = _context.TblServerProperties.FromSqlRaw(@"select 1 as 'Id',getdate() as 'serverdatetimelocal',getutcdate() as 'serverdatetimeutc'").ToList();
            return serverPorp[0].ServerDateTimeUtc;
        }

        /// <summary>
        /// Gets current server date time in server local zone format
        /// </summary>
        /// <returns></returns>
        [HttpGet("currentdatetime/localzone")]
        [ProducesResponseType(typeof(DateTime), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public DateTime GetLocal()
        {
            List<TblServerProperties> serverPorp = _context.TblServerProperties.FromSqlRaw(@"select 1 as 'Id',getdate() as 'serverdatetimelocal',getutcdate() as 'serverdatetimeutc'").ToList();
            return serverPorp[0].ServerDateTimeLocal;
        }

        /// <summary>
        /// Gets current server date time as a unix timestamp
        /// </summary>
        /// <returns></returns>
        [HttpGet("currentdatetime/unixtimestamp")]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public long GetTimestamp()
        {
            List<TblServerProperties> serverPorp = _context.TblServerProperties.FromSqlRaw(@"select 1 as 'Id',getdate() as 'serverdatetimelocal',getutcdate() as 'serverdatetimeutc'").ToList();
            return ((DateTimeOffset)serverPorp[0].ServerDateTimeLocal).ToUnixTimeMilliseconds();
            
            //return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }


        

    }
}
