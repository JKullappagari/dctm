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
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [Route("api/[controller]")]
    public class DownloadsController : Controller
    {
        private readonly DCTrackContext _context;

        public DownloadsController(DCTrackContext context)
        {
            _context = context;

        }

        // Gets to download sqlite db file based on guid
        //GET api/download/12345abc
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Download(string id)
        //{
        //    var stream = await { { } }
        //    var response = File(stream, "application/octet-stream"); // FileStreamResult
        //    return response;
        //}

    }
}
