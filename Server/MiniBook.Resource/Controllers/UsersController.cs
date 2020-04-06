using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniBook.Data.Repositories;

namespace MiniBook.Resource.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery]string name)
        {
            return this.OkResult(_userRepository.SearchAsync(name));
        }
    }
}
