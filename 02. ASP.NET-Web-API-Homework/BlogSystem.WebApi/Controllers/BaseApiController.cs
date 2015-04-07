using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BlogSystem.Data;

namespace BlogSystem.WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        private IBlogSystemData data;

        public BaseApiController() : this(new BlogSystemData(new BlogSystemDbContext()))
        {
        }
        protected BaseApiController(IBlogSystemData data)
        {
            this.data = data;
        }

        protected IBlogSystemData Data
        {
            get
            {
                return this.data;
            }
        }
    }
}