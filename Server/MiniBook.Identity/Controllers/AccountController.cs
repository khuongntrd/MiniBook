using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MiniBook.Data.Repositories;
using MiniBook.Identity.Models;
using MiniBook.Identity.ViewModels;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        [HttpGet("registerFake")]
        public async Task<IActionResult> RegisterWithFakeData()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://randomuser.me/api/?results=100&nat=gb,us&inc=gender,name,email,picture");

                var results = JsonConvert.DeserializeObject<JObject>(response).Value<JArray>("results");

                string UpFirst(string input)
                {
                    return char.ToUpper(input[0]) + input.Substring(1);
                }

                foreach (var randUser in results)
                {
                    var gender = UpFirst(randUser.Value<string>("gender"));
                    var first = UpFirst(randUser.SelectToken("name.first").Value<string>());
                    var last = UpFirst(randUser.SelectToken("name.last").Value<string>());
                    var email = randUser.Value<string>("email");
                    var picture = randUser.SelectToken("picture.large").Value<string>();

                    var model = new RegisterViewModel()
                    {
                        Email = email,
                        Firstname = first,
                        Lastname = last,
                        Gender = gender,
                        Image = picture,
                        Password = "abc!123"
                    };

                    await Register(model);
                }
            }

            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return this.ErrorResult(ErrorCode.REGISTER_REQUIRED_EMAIL);

            if (string.IsNullOrEmpty(model.Firstname))
                return this.ErrorResult(ErrorCode.REGISTER_REQUIRED_FIRST_NAME);

            if (string.IsNullOrEmpty(model.Lastname))
                return this.ErrorResult(ErrorCode.REGISTER_REQUIRED_LAST_NAME);

            var user = new User() { Email = model.Email, UserName = model.Email };

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.GivenName,
                ClaimValue = model.Firstname
            });
            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.FamilyName,
                ClaimValue = model.Lastname
            });
            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Gender,
                ClaimValue = model.Gender.ToString()
            });
            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.BirthDate,
                ClaimValue = model.BirthDate.ToString("yyyy-MM-dd")
            });

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await HttpContext.RequestServices.GetRequiredService<UserRepository>()
                    .CreateAsync(user.Id, model.Firstname + " " + model.Lastname, model.Gender, model.Image);

                return this.OkResult();
            }
            else
            {
                if (result.Errors.Any(x => x.Code == "DuplicateUserName"))
                {
                    return this.ErrorResult(ErrorCode.REGISTER_DUPLICATE_USER_NAME);
                }
                return this.ErrorResult(ErrorCode.BAD_REQUEST);
            }
        }
    }
}
