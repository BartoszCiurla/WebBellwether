using System.Web.Http;
using WebBellwether.API.Utility;
using WebBellwether.Models.Models.Joke;
using WebBellwether.Models.Results;
using WebBellwether.Services.Services.JokeService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Jokes")]
    public class JokesController : ApiController
    {
        private readonly IJokeService _service;
        public JokesController()
        {
            _service = ServiceFactory.JokeService;
        }
        [AllowAnonymous]
        [Route("GetJokes")]
        public IHttpActionResult GetJokes(int language)
        {
            return Ok(_service.GetJokes(language));
        }
        [Authorize(Roles = "Admin")]
        [Route("PostJoke")]
        public IHttpActionResult PostJoke(JokeModel jokeModel)
        {
            ResultStateContainer result = _service.InsertJoke(jokeModel);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString() + (result.ResultValue == null ? "" : "," + result.ResultValue));                      
        }
        [Authorize(Roles = "Admin")]
        [Route("PostDeleteJoke")]
        public IHttpActionResult PostDeleteJoke(JokeModel jokeModel)
        {
            ResultStateContainer result = _service.DeleteJoke(jokeModel);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditJoke")]
        public IHttpActionResult PostEditJoke(JokeModel jokeModel)
        {
            ResultStateContainer result = _service.PutJoke(jokeModel);
            //tutaj jest pewnien problem mianowicie mechanizm do zwracania języków dla kturych nie ma translacji ... 
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString() + (result.ResultValue == null ? "": "," +result.ResultValue));          
        }
        [AllowAnonymous]
        [Route("GetJokeTranslation")]
        public IHttpActionResult GetJokeTranslation(int id,int languageId)
        {
            return Ok(_service.GetJokeTranslation(id,languageId));
        }
        [Authorize(Roles = "Admin")]
        [Route("PostJokeCategory")]
        public IHttpActionResult PostJokeCategory(JokeCategoryModel categoryModel)
        {
            ResultStateContainer result= _service.InsertJokeCategory(categoryModel);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }

        [Authorize(Roles = "Admin")]
        [Route("GetJokeCategoriesWithAvailableLanguage")]
        public IHttpActionResult GetJokeCategoriesWithAvailableLanguage(int language)
        {
            return Ok(_service.GetJokeCategoriesWithAvailableLanguage(language));
        }
        [Authorize(Roles = "Admin")]
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
        public IHttpActionResult GetJokeCategoryTranslation(int id, int languageId)
        {
            return Ok(_service.GetJokeCategoryTranslation(id,languageId));
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditJokeCategory")]

        public IHttpActionResult PostEditJokeCategory(JokeCategoryModel jokeCategory)
        {
            ResultStateContainer result = _service.PutJokeCategory(jokeCategory);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }
        [Authorize(Roles = "Admin")]
        [Route("PostDeleteJokeCategory")]
        public IHttpActionResult PostDeleteJokeCategory(JokeCategoryModel jokeCategory)
        {
            ResultStateContainer result = _service.DeleteJokeCategory(jokeCategory);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());      
        }
    }
}
