using BlogSystem.Data;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BlogSystem.WebApi.Controllers
{
    [RoutePrefix("api/tags")]
    public class TagsController : BaseApiController
    {
        public TagsController(IBlogSystemData data)
            : base(data)
        {
        }
        public TagsController()
            : base()
        {
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var tags = this.Data.Tags.All().Select(x => x.Name);
            if (tags == null)
            {
                return this.NotFound();
            }

            return this.Ok(tags);
        }

        [HttpGet]
        [Route("{tagId:int}")]
        public IHttpActionResult GetById(int tagId)
        {
            var tag = this.Data.Tags.GetById(tagId);
            if (tag == null)
            {
                return this.NotFound();
            }

            return this.Ok(tag.Name);
        }

        [HttpGet]
        [Route("{tagId:int}/posts")]
        public IHttpActionResult GetTagPosts(int tagId)
        {
            var tag = this.Data.Tags.GetById(tagId);
            if (tag == null)
            {
                return this.NotFound();
            }

            var tagPosts = tag.Posts.Select(x => x.Title);
            return this.Ok(tagPosts);
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddTag(Tag tag)
        {
            var addedTag = this.Data.Tags.Add(tag);
            this.Data.SaveChanges();
            return this.Ok(addedTag);
        }

        [HttpPut]
        [Route("edit/{tagId:int}")]
        public IHttpActionResult EditTag(int tagId, Tag tag)
        {
            if (tagId != tag.Id)
            {
                return this.BadRequest("Can not edit this tag");
            }

            var editedTag = this.Data.Tags.Update(tag);
            this.Data.SaveChanges();
            return this.Ok(editedTag);
        }

        [HttpDelete]
        [Route("delete/{tagId:int}")]
        public IHttpActionResult DeleteTag(int tagId)
        {
            var tag = this.Data.Tags.Find(t => t.Id == tagId).FirstOrDefault();
            if (tag == null)
            {
                return this.NotFound();
            }

            this.Data.Tags.Delete(tag);
            this.Data.SaveChanges();
            return this.Ok(tag.Id);
        }
    }
}