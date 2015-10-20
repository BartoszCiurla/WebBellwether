using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Services.JokeService;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.Results;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Jokes")]
    public class JokesController : ApiController
    {
        private readonly JokeService _service;
        public JokesController()
        {
            _service = new JokeService(new JokeUnitOfWork());
        }
        [Authorize]
        [Route("PostJokeCategory")]
        public IHttpActionResult PostJokeCategory(JokeCategoryModel categoryModel)
        {
            ResultStateContainer result= _service.InsertJokeCategory(categoryModel);
            switch (result.ResultState)
            {
                case ResultState.JokeCategoryAdded:
                    return Ok();
                case ResultState.JokeCategoryExistsInDb:
                    return BadRequest("JokeCategoryExistsInDb");
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CriticalError");
            }
        }

        [Authorize]
        [Route("GetJokeCategoriesWithAvailableLanguage")]
        public IHttpActionResult GetJokeCategoriesWithAvailableLanguage(int language)
        {
            return Ok(_service.GetJokeCategoriesWithAvailableLanguage(language));
        }

    }
}
