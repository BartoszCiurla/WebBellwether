using System.Web.Http;
using WebBellwether.API.Services.JokeService;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.Results;
using WebBellwether.API.Services.JokeService.Abstract;
using WebBellwether.API.Repositories;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Jokes")]
    public class JokesController : ApiController
    {
        private readonly IJokeService _service;
        public JokesController()
        {
            _service = new JokeService(new AggregateRepositories());
        }

        [Authorize]
        [Route("PostJoke")]
        public IHttpActionResult PostJoke(JokeModel jokeModel)
        {
            ResultStateContainer result = _service.InsertJoke(jokeModel);
            switch (result.ResultState)
            {
                case ResultState.JokeAdded:
                    return Ok(result.Value);
                case ResultState.JokeTranslationEdited:
                    return Ok();
                case ResultState.JokeExists:
                    return BadRequest("JokeExists");
                case ResultState.JokeCategoryNotExistsInDb:
                    return BadRequest("JokeCategoryNotExists");
                case ResultState.LanguageNotExists:
                    return BadRequest("LanguageNotExists");
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CriticalError");
            }
            
        }
        [Authorize]
        [Route("PostDeleteJoke")]
        public IHttpActionResult PostDeleteJoke(JokeModel jokeModel)
        {
            ResultStateContainer result = _service.DeleteJoke(jokeModel);
            switch (result.ResultState)
            {
                case ResultState.JokeDeleted:
                    return Ok();
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                case ResultState.JokeDetailNotExists:
                    return BadRequest("JokeDetailNotExists");
                case ResultState.JokeNotExists:
                    return BadRequest("JokeNotExists");
                default:
                    return BadRequest("CriticalError");
            }
        }
        [Authorize]
        [Route("PostEditJoke")]
        public IHttpActionResult PostEditJoke(JokeModel jokeModel)
        {
            ResultStateContainer result = _service.PutJoke(jokeModel);
            switch (result.ResultState)
            {
                case ResultState.JokeNotExists:
                    return BadRequest("JokeNotExists");
                case ResultState.JokeExists:
                    return BadRequest("JokeExists");
                case ResultState.JokeCategoryNotExistsInDb:
                    string message = "JokeCategoryNotExists" + "," + result.Value.ToString();
                    return BadRequest(message);
                case ResultState.JokeEdited:
                    return Ok();
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CritialError");
            }
        }
        [AllowAnonymous]
        [Route("GetJokeTranslation")]
        public IHttpActionResult GetJokeTranslation(int id,int languageId)
        {
            return Ok(_service.GetJokeTranslation(id,languageId));
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
        [Authorize]
        [Route("GetJokesWithAvailableLanguages")]
        public IHttpActionResult GetJokesWithAvailableLanguages(int language)
        {
            return Ok(_service.GetJokesWithAvailableLanguages(language));
        }
        [AllowAnonymous]
        [Route("GetJokeCategories")]
        public IHttpActionResult GetJokeCategories(int language)
        {
            return Ok(_service.GetJokeCategories(language));
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
