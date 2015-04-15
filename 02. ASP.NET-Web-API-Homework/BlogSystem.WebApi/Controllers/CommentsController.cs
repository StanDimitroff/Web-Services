using BlogSystem.Data;
using BlogSystem.Models;
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
            var comments = this.Data.Comments.All().Select(x => new
            {
                x.Content,
                x.User.Username
            });

            if (comments == null)
            {
                return this.NotFound();
            }

            return this.Ok(comments);
        }

        [HttpGet]
        [Route("{commentId:int}")]
        public IHttpActionResult GetById(int commentId)
        {
            var comment = this.Data.Comments.GetById(commentId);
            if (comment == null)
            {
                return this.NotFound();
            }

            return this.Ok(comment);
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddComment(Comment comment)
        {
            var addedComment = this.Data.Comments.Add(comment);
            this.Data.SaveChanges();
            return this.Ok(addedComment);
        }

        [HttpPut]
        [Route("edit/{commentId:int}")]
        public IHttpActionResult EditPost(int commentId, Comment comment)
        {
            if (commentId != comment.Id)
            {
                return this.BadRequest("Can not edit this comment");
            }

            var editedComment = this.Data.Comments.Update(comment);
            this.Data.SaveChanges();
            return this.Ok(editedComment);
        }

        [HttpDelete]
        [Route("delete/{commentId:int}")]
        public IHttpActionResult DeleteComment(int commentId)
        {
            var comment = this.Data.Comments.Find(c => c.Id == commentId).FirstOrDefault();
            if (comment == null)
            {
                return this.NotFound();
            }

            this.Data.Comments.Delete(comment);
            this.Data.SaveChanges();
            return this.Ok(comment.Id);
        }
    }
}