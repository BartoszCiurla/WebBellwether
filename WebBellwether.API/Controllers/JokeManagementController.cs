using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.Joke;
using WebBellwether.Services.Services.JokeService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/JokeManagement")]
    public class JokeManagementController : ApiController
    {
        private readonly IJokeManagementService _jokeManagementService;

        public JokeManagementController(IJokeManagementService jokeManagementService)
        {
            _jokeManagementService = jokeManagementService;
        }
        [Authorize(Roles = "Admin")]
        [Route("PostJoke")]
        public JsonResult<ResponseViewModel<JokeViewModel>> PostJoke(JokeViewModel joke)
        {
            var response = ServiceExecutor.Execute(() => _jokeManagementService.InsertJoke(joke));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostDeleteJoke")]
        public JsonResult<ResponseViewModel<bool>> PostDeleteJoke(JokeViewModel joke)
        {
            var response = ServiceExecutor.Execute(() => _jokeManagementService.RemoveJoke(joke));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditJoke")]
        public JsonResult<ResponseViewModel<bool>> PostEditJoke(JokeViewModel joke)
        {
            var response = ServiceExecutor.Execute(() => _jokeManagementService.PutJoke(joke));
            return Json(response);
        }
        [AllowAnonymous]
        [Route("GetJokeTranslation")]
        public JsonResult<ResponseViewModel<JokeViewModel>> GetJokeTranslation(int jokeId, int languageId)
        {
            var response = ServiceExecutor.Execute(() => _jokeManagementService.GetJokeTranslation(jokeId, languageId));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("GetJokesWithAvailableLanguages")]
        public JsonResult<ResponseViewModel<JokeViewModel[]>> GetJokesWithAvailableLanguages(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => _jokeManagementService.GetJokesWithAvailableLanguages(languageId));
            return Json(response);
        }
    }
}
