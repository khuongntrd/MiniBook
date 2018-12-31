using Microsoft.AspNetCore.Mvc;
using MiniBook.Resource.Data;
using MiniBook.Resource.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniBook.Resource.Controllers
{
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
            _dataContext.Posts.InsertOne(post);

            return this.OkResult();
        }
    }
}
