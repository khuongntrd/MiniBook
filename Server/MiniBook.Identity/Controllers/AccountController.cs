using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBook.Identity.Models;
using MiniBook.Identity.ViewModels;

namespace MiniBook.Identity.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        UserManager<User> UserManager { get; }

        public AccountController(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            var user = new User() { Email = model.Email, UserName = model.Email };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return this.OkResult();
            else
                return this.ErrorResult(1, "abc");
        }
    }
}
