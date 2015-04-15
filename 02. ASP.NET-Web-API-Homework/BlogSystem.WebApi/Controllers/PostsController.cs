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

            if (posts == null)
            {
                return this.NotFound();
            }

            return this.Ok(posts);
        }

        [HttpGet]
        [Route("{postId:int}")]
        public IHttpActionResult GetById(int postId)
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
                return this.BadRequest("No post found");
            }

            var postTags = post.Tags.Select(x => x.Name);
            return this.Ok(postTags);
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddPost(Post post)
        {
            var addedPost = this.Data.Posts.Add(post);
            this.Data.SaveChanges();
            return this.Ok(addedPost);
        }


        [HttpPost]
        [Route("edit/{postId:int}")]
        public IHttpActionResult EditPost(int postId, Post post)
        {
            if (postId != post.Id)
            {
                return this.BadRequest("Can not edit this post");
            }

            var editedPost = this.Data.Posts.Update(post);
            this.Data.SaveChanges();
            return this.Ok(editedPost);
        }

        [HttpDelete]
        [Route("delete/{postId:int}")]
        public IHttpActionResult DeletePost(int postId)
        {
            var post = this.Data.Posts.Find(p => p.Id == postId).FirstOrDefault();
            if (post == null)
            {
                return this.NotFound();
            }

            this.Data.Posts.Delete(post);
            this.Data.SaveChanges();
            return this.Ok(post.Id);
        }
    }
}