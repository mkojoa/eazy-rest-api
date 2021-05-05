using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Controllers
{
    [Route("api/[controller]")]
    public class RestController : BaseController
    {
        public RestController()
        {

        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}
