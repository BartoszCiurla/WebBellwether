using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.Joke;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/JokeManagement")]
    public class JokeManagementController : ApiController
    {
        [Authorize(Roles = "Admin")]
        [Route("PostJoke")]
        public JsonResult<ResponseViewModel<JokeViewModel>> PostJoke(JokeViewModel joke)
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.JokeManagementService.InsertJoke(joke));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostDeleteJoke")]
        public JsonResult<ResponseViewModel<bool>> PostDeleteJoke(JokeViewModel joke)
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.JokeManagementService.RemoveJoke(joke));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditJoke")]
        public JsonResult<ResponseViewModel<bool>> PostEditJoke(JokeViewModel joke)
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.JokeManagementService.PutJoke(joke));
            return Json(response);
        }
        [AllowAnonymous]
        [Route("GetJokeTranslation")]
        public JsonResult<ResponseViewModel<JokeViewModel>> GetJokeTranslation(int jokeId, int languageId)
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.JokeManagementService.GetJokeTranslation(jokeId, languageId));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("GetJokesWithAvailableLanguages")]
        public JsonResult<ResponseViewModel<JokeViewModel[]>> GetJokesWithAvailableLanguages(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.JokeManagementService.GetJokesWithAvailableLanguages(languageId));
            return Json(response);
        }
    }
}
