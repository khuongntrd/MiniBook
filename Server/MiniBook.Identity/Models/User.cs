using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MiniBook.Identity.Models
{
    public class User : IdentityUser
    {
        public ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
    }
}
