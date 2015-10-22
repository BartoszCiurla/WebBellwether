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
                    return Ok(result.Value);
                case ResultState.JokeCategoryExistsInDb:
                    return BadRequest("JokeCategoryExistsInDb");
                case ResultState.JokeCategoryTranslationAdded:
                    return Ok();
                case ResultState.JokeCategoryTranslationEdited:
                    return Ok();
                case ResultState.JokeCategoryTranslationNotExists:
                    return BadRequest();
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
        [AllowAnonymous]
        [HttpGet]
        [Route("GetJokeCategoryTranslation")]
        public IHttpActionResult GetJokeCategoryTranslation(int id, int languageId)//here i have game main id and language id 
        {
            return Ok(_service.GetJokeCategoryTranslation(id,languageId));
        }
        [Authorize]
        [Route("PostEditJokeCategory")]

        public IHttpActionResult PostEditJokeCategory(JokeCategoryModel jokeCategory)
        {
            ResultStateContainer result = _service.PutJokeCategory(jokeCategory);
            switch (result.ResultState)
            {
                case ResultState.JokeCategoryEdited:
                    return Ok();
                case ResultState.JokeCategoryNotExistsInDb:
                    return BadRequest("JokeCategoryNotExists");
                case ResultState.ThisJokeCategoryExists:
                    return BadRequest("ThisJokeCategoryExists");
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CriticalError");
            }
        }
        [Authorize]
        [Route("PostDeleteJokeCategory")]
        public IHttpActionResult PostDeleteJokeCategory(JokeCategoryModel jokeCategory)
        {
            ResultStateContainer result = _service.DeleteJokeCategory(jokeCategory);
            switch (result.ResultState)
            {
                case ResultState.JokeCategoryDeleted:
                    return Ok(result);
                case ResultState.JokeCategoryTranslationNotExists:
                    return BadRequest("JokeCategoryTranslationNotExists");
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CriticalError");
            }
        }

    }
}
