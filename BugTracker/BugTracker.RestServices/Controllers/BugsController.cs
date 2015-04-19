using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugTracker.Data;
using BugTracker.RestServices.Models;
using BugTracker.Data.Models;
using Microsoft.AspNet.Identity;
namespace BugTracker.RestServices.Controllers
{
    [RoutePrefix("api/bugs")]
    public class BugsController : ApiController
    {
        private BugTrackerDbContext db = new BugTrackerDbContext();

        [HttpGet]
        public IHttpActionResult GetAllBugs()
        {
            var bugs = db.Bugs.OrderByDescending(b => b.DateCreated).Select(b => new
            {
                b.Id,
                b.Title,
                Status = b.Status.ToString(),
                Author = b.Author.UserName,
                b.DateCreated
            });

            return this.Ok(bugs);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetSingleBug(int id)
        {
            var bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }

            return this.Ok(new
            {
                bug.Id,
                bug.Title,
                bug.Description,
                Status = bug.Status.ToString(),
                Author = bug.Author != null ? bug.Author.UserName : null,
                bug.DateCreated,
                Comments = bug.Comments.OrderByDescending(c => c.DateCreated).Select(c => new
                {
                    c.Id,
                    c.Text,
                    c.DateCreated
                })
            });
        }

        [HttpPost]
        public IHttpActionResult SubmitBug(BugBindingModel bugData)
        {
            if (bugData.Title == null)
            {
                return this.BadRequest();
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.db.Users.Find(currentUserId);

            var bug = new Bug
            {
                Title = bugData.Title,
                Description = bugData.Description,
                Status = BugStatus.Open,
                DateCreated = DateTime.Now,
                Author = currentUser
            };

            db.Bugs.Add(bug);
            db.SaveChanges();

            if (bug.Author == null)
            {
                return this.CreatedAtRoute(
               "DefaultApi",
               new { controller = "bugs", id = bug.Id },
               new { bug.Id, Message = "Anonymous bug submitted." });
            }

            return this.CreatedAtRoute(
              "DefaultApi",
              new { controller = "bugs", id = bug.Id },
              new { bug.Id, Author = currentUser.UserName, Message = "User bug submitted." });
        }

        [HttpPatch]
        [Route("{id:int}")]
        public IHttpActionResult EditBug([FromUri]int id, [FromBody]EditBugBindingModel bugData)
        {
            if (bugData == null)
            {
                return this.BadRequest();
            }

            var bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }

            if (bugData.Title != null)
            {
                bug.Title = bugData.Title;
            }

            if (bugData.Description != null)
            {
                bug.Description = bugData.Description;
            }

            if (bugData.Status != null)
            {
                bug.Status = (BugStatus)Enum.Parse(typeof(BugStatus), bugData.Status);
            }

            db.SaveChanges();

            return this.Ok(new
            {
                Message = string.Format("Bug #{0} patched.", id)
            });

        }


        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteBug(int id)
        {
            var bug = db.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }

            db.Bugs.Remove(bug);
            db.SaveChanges();
            return this.Ok(new
                {
                    Message = string.Format("Bug #{0} deleted.", id)
                });
        }


        [HttpGet]
        [Route("filter")]
        public IHttpActionResult ListBugsByFilter([FromUri]FilterBugBindingModel filterData)
        {
            IQueryable<Bug> bugs = null;
            var keyword = filterData.Keyword != null ? filterData.Keyword : null;
            string statuses = filterData.Statuses != null ? filterData.Statuses: null;
            var author = filterData.Author != null ? filterData.Author : null;

            if (keyword != null)
            {
                bugs = db.Bugs.Where(b => b.Title.Contains(keyword));
            }

            if (statuses != null)
            {
                if (bugs != null)
                {
                    bugs = bugs.Where(b => statuses.Contains(b.Status.ToString()));
                }
                else
                {
                    bugs = db.Bugs.Where(b => statuses.Contains(b.Status.ToString()));
                }
            }

            if (author != null)
            {
                if (bugs != null)
                {
                    bugs = bugs.Where(b => b.Author.UserName == author);
                }
                else
                {
                    bugs = db.Bugs.Where(b => b.Author.UserName == author);
                }
            }

            return this.Ok(bugs.OrderByDescending(b => b.DateCreated).Select(b => new
            {
                b.Id,
                b.Title,
                Status = b.Status.ToString(),
                Author = b.Author.UserName,
                b.DateCreated
            }));
        }
    }
}