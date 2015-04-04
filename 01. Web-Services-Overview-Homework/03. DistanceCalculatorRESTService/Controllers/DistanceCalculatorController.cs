using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DistanceCalculatorRESTService.Models;

namespace DistanceCalculatorRESTService.Controllers
{
    public class DistanceController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetDistance(int sx, int sy, int ex, int ey)
        {
            Point startPoint = new Point()
            {
                X = sx,
                Y = sy
            };
            Point endPoint = new Point()
            {
                X = ex,
                Y = ey
            };

            return this.Ok(Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2)));
        }
    }
}