using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Messages.Data;
using Messages.Data.Models;

namespace Messages.RestServices.Controllers
{
    [RoutePrefix("api/channels")]
    public class ChannelsController : ApiController
    {
        private MessagesDbContext db = new MessagesDbContext();

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var channels = db.Channels.OrderBy(c => c.Name).ToList();
            return this.Ok(channels);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetChannel(int id)
        {
            var channel = db.Channels.Find(id);
            if (channel == null)
            {
                return this.NotFound();
            }

            return this.Ok(channel);
        }

        //[HttpPost]
        //public IHttpActionResult AddChannel(Channel channel)
        //{
        //    if (channel.Name == null)
        //    {
        //        return this.BadRequest();
        //    }
        //    if (db.Channels.Where(c => c.Name == channel.Name) != null)
        //    {
        //        return this.Conflict();
        //    }

        //    db.Channels.Add(channel);
        //    db.SaveChanges();
        //    return this.CreatedAtRoute(
        //         "DefaultApi",
        //         new { controller = "channels", id = channel.Id },
        //         new { channel.Id, channel.Name });
        //}
    }
}
