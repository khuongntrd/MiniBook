using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBook.Data.Repositories;
using MiniBook.Identity.Models;
using MiniBook.Identity.ViewModels;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MiniBook.Identity.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        UserManager<User> UserManager { get; }

        private readonly UserRepository _userRepository;

        public AccountController(UserManager<User> userManager, UserRepository userRepository)
        {
            UserManager = userManager;
            _userRepository = userRepository;
        }

        [HttpGet("seed")]
        public async Task<IActionResult> Seed()
        {
            string UpFirst(string s)
            {
                return char.ToUpper(s[0]) + s.Substring(1);
            }

            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync(
                     "https://randomuser.me/api/?results=999&nat=gb,us&inc=gender,name,email,picture");

                foreach (var doc in (JArray)JObject.Parse(json)["results"])
                {
                    var vm = new RegisterViewModel()
                    {
                        Gender = UpFirst(doc.Value<string>("gender")),
                        Firstname = UpFirst(doc.SelectToken("name.first").Value<string>()),
                        Lastname = UpFirst(doc.SelectToken("name.last").Value<string>()),
                        Email = doc.Value<string>("email"),
                        Image = doc.SelectToken("picture.large").Value<string>(),
                        Password = "Abc@123",
                        BirthDate = new System.DateTime(1990, 1, 1)
                    };

                    await Register(vm);
                }
            }

            return this.OkResult();
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
                ClaimValue = model.Gender
            });
            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.BirthDate,
                ClaimValue = model.BirthDate.Date.ToString("yyyy-MM-dd")
            });
            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Picture,
                ClaimValue = model.Image
            });
            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userRepository.CreateAsync(user.Id, model.Firstname + " " + model.Lastname, model.Gender, model.Image);

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
