using BlogSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BlogSystem.Models;

namespace BlogSystem.WebApi.Controllers
{
    [RoutePrefix("api/posts")]
    public class PostsController : BaseApiController
    {
        public PostsController(IBlogSystemData data)
            : base(data)
        {
        }
        public PostsController()
            : base()
        {
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var posts = this.Data.Posts.All().Select(x => new 
            {
                x.Title,
                x.Content,
                x.User.Username,
                Tags = x.Tags.Select(t => t.Name)
            });

            return this.Ok(posts);
        }

        [HttpGet]
        [Route("{postId:int}")]
        public IHttpActionResult GetPost(int postId)
        {
            var post = this.Data.Posts.GetById(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            return this.Ok(post);
        }   

        [HttpGet]
        [Route("{postId:int}/tags")]
        public IHttpActionResult GetPostTags(int postId)
        {
            var post = this.Data.Posts.GetById(postId);
            if (post == null)
            {
                return this.BadRequest("No such post");
            }

            var postTags = post.Tags.Select(x => x.Name);
            return this.Ok(postTags);
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddPost([FromBody]int userId, [FromBody]string title, [FromBody]string content)
        {
            var user = this.Data.Users.GetById(userId);
            if (user == null)
            {
                return this.BadRequest("No such user");
            }
            var post = new Post()
            {
                Title = title,
                Content = content,
                UserId = userId,
                User = user
            };

            var a = this.Data.Posts.Add(post);
            return this.Ok(a.Id);
        }
    }
}