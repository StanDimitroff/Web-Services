namespace BlogSystem.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using BlogSystem.Data;
    using BlogSystem.Data.Repositories;
    using BlogSystem.Models;

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
                Gender = x.Gender.ToString(),
                x.ContactInfo
            });
            if (users == null)
            {
                return this.NotFound();
            }

            return this.Ok(users);
        }

        [HttpGet]
        [Route("{gender:regex(Male)}")]
        public IHttpActionResult GetAllByGender(string gender)
        {
            Gender g = (Gender)Enum.Parse(typeof(Gender), gender, true);
            var users = this.Data.Users.AllByGender(g).Select(x => new
            {
                x.Id,
                x.Username,
                x.FullName,
                x.Birthday,
                Gender = x.Gender.ToString(),
                x.ContactInfo
            });
            if (users == null)
            {
                return this.NotFound();
            }

            return this.Ok(users);
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
                Gender = x.Gender.ToString(),
                x.ContactInfo
            });
            if (authors == null)
            {
                return this.NotFound();
            }

            return this.Ok(authors);
        }

        [HttpGet]
        [Route("{userId:int}")]
        public IHttpActionResult GetById(int userId)
        {
            var user = this.Data.Users.GetById(userId);

            if (user == null)
            {
                return this.NotFound();
            }
            var u = new
            {
                Username = user.Username,
                FullName = user.FullName,
                BirthDay = user.Birthday,
                Gender = user.Gender.ToString(),
                Facebook = user.ContactInfo.Facebook,
                Skype = user.ContactInfo.Skype,
                Tweeter = user.ContactInfo.Tweeter,
                PhoneNumber = user.ContactInfo.PhoneNumber,
                Posts = user.Posts.Select(p => p.Title),
                Comments = user.Comments.Select(c => c.Content)
            };

            return this.Ok(u);
        }

        [HttpGet]
        [Route("{username}")]
        public IHttpActionResult GetByUsername(string username)
        {
            var user = this.Data.Users.GetUserByUsername(username);
            if (user == null)
            {
                return this.NotFound();
            }

            var u = new
            {
                Username = user.Username,
                FullName = user.FullName,
                BirthDay = user.Birthday,
                Gender = user.Gender.ToString(),
                Facebook = user.ContactInfo.Facebook,
                Skype = user.ContactInfo.Skype,
                Tweeter = user.ContactInfo.Tweeter,
                PhoneNumber = user.ContactInfo.PhoneNumber,
                Posts = user.Posts.Select(p => p.Title),
                Comments = user.Comments.Select(c => c.Content)
            };

            return this.Ok(u);
        }

        [HttpGet]
        [Route("{userId:int}/posts")]
        public IHttpActionResult GetUserPosts(int userId)
        {
            var user = this.Data.Users.GetById(userId);
            if (user == null)
            {
                return this.BadRequest("No user found");
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
                return this.BadRequest("No user found");
            }

            var userComments = this.Data.Users.GetById(userId).Comments.Select(x => new
            {
                x.Content
            });

            return this.Ok(userComments);
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddUser(User user)
        {
            var addedUser = this.Data.Users.Add(user);
            this.Data.SaveChanges();
            return this.Ok(addedUser);
        }

        [HttpPut]
        [Route("edit/{userId:int}")]
        public IHttpActionResult EditUser(int userId, User user)
        {
            if (userId != user.Id)
            {
                return this.BadRequest("Can not edit this user");
            }

            var updatedUser = this.Data.Users.Update(user);
            this.Data.SaveChanges();
            return this.Ok(updatedUser);
            
        }

        [HttpDelete]
        [Route("delete/{userId:int}")]
        public IHttpActionResult DeleteUser(int userId)
        {
            var user = this.Data.Users.Find(u => u.Id == userId).FirstOrDefault();
            if (user == null)
            {
                return this.NotFound();
            }

            this.Data.Users.Delete(user);
            this.Data.SaveChanges();
            return this.Ok(user.Id);
        }

    }
}