using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBellwether.API.Context;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Language")]
    public class LanguageController : ApiController
    {
        private EfDbContext _ctx = new EfDbContext();
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_ctx.Languages.ToList());
        }
    }
}
