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

        [Route("GetJokes")]
        public IHttpActionResult GetJokes(int language)
        {
            return Ok(_service.GetJokes(language));
        }
        [Authorize]
        [Route("PostJoke")]
        public IHttpActionResult PostJoke(JokeModel jokeModel)
        {
            ResultStateContainer result = _service.InsertJoke(jokeModel);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString() + (result.ResultValue == null ? "" : "," + result.ResultValue));                      
        }
        [Authorize]
        [Route("PostDeleteJoke")]
        public IHttpActionResult PostDeleteJoke(JokeModel jokeModel)
        {
            ResultStateContainer result = _service.DeleteJoke(jokeModel);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }
        [Authorize]
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
        [Authorize]
        [Route("PostJokeCategory")]
        public IHttpActionResult PostJokeCategory(JokeCategoryModel categoryModel)
        {
            ResultStateContainer result= _service.InsertJokeCategory(categoryModel);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
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
        public IHttpActionResult GetJokeCategoryTranslation(int id, int languageId)
        {
            return Ok(_service.GetJokeCategoryTranslation(id,languageId));
        }
        [Authorize]
        [Route("PostEditJokeCategory")]

        public IHttpActionResult PostEditJokeCategory(JokeCategoryModel jokeCategory)
        {
            ResultStateContainer result = _service.PutJokeCategory(jokeCategory);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }
        [Authorize]
        [Route("PostDeleteJokeCategory")]
        public IHttpActionResult PostDeleteJokeCategory(JokeCategoryModel jokeCategory)
        {
            ResultStateContainer result = _service.DeleteJokeCategory(jokeCategory);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());      
        }
    }
}
