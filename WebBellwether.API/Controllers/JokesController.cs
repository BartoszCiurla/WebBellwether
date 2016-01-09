using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.Joke;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Jokes")]
    public class JokesController : ApiController
    {
        [AllowAnonymous]
        [Route("GetJokes")]
        public JsonResult<ResponseViewModel<JokeViewModel[]>> GetJokes(int languageId)
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.JokeService.GetJokes(languageId));
            return Json(response);
        }  

        [Authorize(Roles = "Admin")]
        [Route("GetJokeCategoriesWithAvailableLanguage")]
        public JsonResult<ResponseViewModel<JokeCategoryViewModel[]>> GetJokeCategoriesWithAvailableLanguage(int languageId)
        {
            var response =
                ServiceExecutor.Execute(
                    () => ServiceFactory.JokeService.GetJokeCategoriesWithAvailableLanguage(languageId));
                return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("GetJokesWithAvailableLanguages")]
        public JsonResult<ResponseViewModel<JokeViewModel[]>> GetJokesWithAvailableLanguages(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.JokeService.GetJokesWithAvailableLanguages(languageId));
                return Json(response);
        }
    }
}
