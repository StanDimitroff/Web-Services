using BlogSystem.Data;
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
        public IHttpActionResult GetTag(int tagId)
        {
            var tag = this.Data.Tags.GetById(tagId);
            if (tag == null)
            {
                return this.NotFound();
            }

            return this.Ok(tag);
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
            return this.Ok(tag);
        }
    }
}