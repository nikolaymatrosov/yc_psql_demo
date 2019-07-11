using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using yc_psql_demo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace yc_psql_demo.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly PostContext _context;
        public PostsController(PostContext context)
        {
            _context = context;
        }
        // GET: api/posts
        [HttpGet]
        public ActionResult<List<PostClass>> Get()
        {
            return _context.Posts.ToList();
        }

        // GET api/posts/5
        [HttpGet("{id}")]
        public ActionResult<PostClass> Get(int id)
        {
            return _context.Posts.Find(id);
        }

        // POST api/posts
        [HttpPost]
        public ActionResult<PostClass> Post([FromBody]PostCreateRequestClass request)
        {
            PostClass post = new PostClass {Heading=request.Heading, Text=request.Text};
            _context.Posts.Add(post);
            _context.SaveChanges();
            return post;
        }

        // PUT api/posts/5
        [HttpPut("{id}")]
        public ActionResult<PostClass> Put(int id, [FromBody]PostUpdateRequestClass request)
        {
            PostClass post = _context.Posts.First(p => p.Id == id);
            post.Text = request.Text;
            post.Heading = request.Heading;
            _context.SaveChanges();
            return post;
        }

        // DELETE api/posts/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            PostClass post = new PostClass { Id = id };
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }
    }
}
