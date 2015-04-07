using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BlogSystem.Data;
using BlogSystem.Data.Repositories;

namespace BlogSystem.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        public UsersController(IBlogSystemData data)
            : base(data)
        {
        }
        public UsersController()
            : base()
        {
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var users = this.Data.Users.All().Select(x => new
            {
                x.Id,
                x.Username,
                x.FullName,
                x.Birthday,
                x.Gender,
                x.ContactInfo
            });
            if (users == null)
            {
                return this.NotFound();
            }

            return this.Ok(users);
        }

        [HttpGet]
        [Route("{userId:int}")]
        public IHttpActionResult GetUser(int userId)
        {
            var user = this.Data.Users.GetById(userId);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.Ok(user);
        }

        [HttpGet]
        [Route("name")]
        public IHttpActionResult GetUserByUsername(string username)
        {
            var user = this.Data.Users.GetUserByUsername(username);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.Ok(user);
        }

        [HttpGet]
        [Route("authors")]
        public IHttpActionResult GetAllAuthors()
        {
            var authors = this.Data.Users.AllAuthors().Select(x => new
            {
                x.Id,
                x.Username,
                x.FullName,
                x.Birthday,
                x.Gender,
                x.ContactInfo
            });

            return this.Ok(authors);
        }

        [HttpGet]
        [Route("{userId:int}/posts")]
        public IHttpActionResult GetUserPosts(int userId)
        {
            var user = this.Data.Users.GetById(userId);
            if (user == null)
            {
                return this.BadRequest("No such user");
            }

            var userPosts = this.Data.Users.GetById(userId).Posts.Select(x => new
                {
                    x.Title,
                    x.Content,
                    Tags = x.Tags.Select(t => t.Name)
                });

            return this.Ok(userPosts);
        }

        [HttpGet]
        [Route("{userId:int}/comments")]
        public IHttpActionResult GetUserComments(int userId)
        {
            var user = this.Data.Users.GetById(userId);
            if (user == null)
            {
                return this.BadRequest("No such user");
            }

            var userComments = this.Data.Users.GetById(userId).Comments.Select(x => new
            {
                x.Content
            });

            return this.Ok(userComments);
        }

        //[HttpPost]
        //[Route("add")]
        //public int AddUser([FromBody]string username, string fullName, string registerDate, string birthdate, string gender, string facebook, string skype, string tweeter, string phoneNumber)
        //{

        //}
        
    }
}