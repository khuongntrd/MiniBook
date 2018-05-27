using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniBook.Identity.Models;

namespace MiniBook.Identity.Data
{
    public class MinibookDbContext : IdentityDbContext<User>
    {
        public MinibookDbContext(DbContextOptions<MinibookDbContext> options) : base(options)
        {

        }
    }
}
