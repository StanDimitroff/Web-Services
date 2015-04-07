using BlogSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BlogSystem.WebApi.Controllers
{
    [RoutePrefix("api/comments")]
    public class CommentsController : BaseApiController
    {
        public CommentsController(IBlogSystemData data)
            : base(data)
        {
        }
        public CommentsController()
            : base()
        {
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var posts = this.Data.Comments.All().Select(x => new
            {
                x.Content,
                x.User.Username,
            });

            return this.Ok(posts);
        }

        [HttpGet]
        [Route("{commentId:int}")]
        public IHttpActionResult GetComment(int commentId)
        {
            var comment = this.Data.Comments.GetById(commentId);
            if (comment == null)
            {
                return this.NotFound();
            }

            return this.Ok(comment);
        }
    }
}