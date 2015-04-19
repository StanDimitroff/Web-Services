using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.RestServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace BugTracker.RestServices.Controllers
{
    [RoutePrefix("api")]
    public class CommentsController : ApiController
    {
        private BugTrackerDbContext db = new BugTrackerDbContext();

        [HttpGet]
        [Route("comments")]
        public IHttpActionResult GetAllComments()
        {
            var comments = db.Comments.OrderByDescending(c => c.DateCreated).Select(c => new
                {
                    c.Id,
                    c.Text,
                    Author = c.Author.UserName,
                    c.DateCreated,
                    BugId = c.Bug.Id,
                    BugTitle = c.Bug.Title
                });

            return this.Ok(comments);
        }

        [HttpGet]
        [Route("bugs/{id:int}/comments")]
        public IHttpActionResult GetBugComments(int id)
        {
            var bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }

            var bugComments = bug.Comments.OrderByDescending(c => c.DateCreated).Select(c => new
            {
                c.Id,
                c.Text,
                Author = c.Author != null ? c.Author.UserName : null,
                c.DateCreated
            });

            return this.Ok(bugComments);
        }


        [HttpPost]
        [Route("bugs/{id:int}/comments")]
        public IHttpActionResult AddComment(int id, CommentBindingModel commentData)
        {
            if (commentData == null)
            {
                return this.BadRequest();
            }

            var bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.db.Users.Find(currentUserId);

            var bugComment = new Comment
            {
                Text = commentData.Text,
                DateCreated = DateTime.Now,
                Author = currentUser,
                Bug = bug
            };

            db.Comments.Add(bugComment);
            db.SaveChanges();
            if (bugComment.Author == null)
            {
                return this.Ok(new {Id = bugComment.Id, Message = string.Format("Added anonymous comment for bug #{0}", id) });
            }

            return this.Ok(new { Id = bugComment.Id, Author = currentUser.UserName ,Message = string.Format("User comment added for bug #{0}", id) });
        }
    }
}
