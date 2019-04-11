using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBook.Resource.Data;
using MiniBook.Resource.Models;
using System;

namespace MiniBook.Resource.Controllers
{
    [Authorize]
    [Route("api/[controller]")]    
    public class PostsController : Controller
    {
        private DataContext _dataContext;

        public PostsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public IActionResult Create([FromBody]Post post)
        {
            post.User = new Profile()
            {
                Id = User.FindFirst("sub").Value
            };

            post.UpdatedAt = DateTime.UtcNow;

            _dataContext.Posts.InsertOne(post);

            return this.OkResult();
        }
    }
}
