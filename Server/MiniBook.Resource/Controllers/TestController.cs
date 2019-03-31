using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBook.Resource.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [Authorize(Policy ="Api1Policy")]
        public IActionResult Get()
        {
            return this.Ok("Hello");
        }
    }

    [Route("api/[controller]")]
    public class Test2Controller : Controller
    {
        [HttpGet]
        [Authorize(Policy = "Api2Policy")]
        public IActionResult Get()
        {
            return this.Ok("Hello2");
        }
    }
}
