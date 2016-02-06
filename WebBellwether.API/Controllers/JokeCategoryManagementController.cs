using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.Joke;
using WebBellwether.Services.Services.JokeService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/JokeCategoryManagement")]
    public class JokeCategoryManagementController : ApiController
    {
        private readonly IJokeCategoryManagementService _jokeCategoryManagementService;

        public JokeCategoryManagementController(IJokeCategoryManagementService jokeCategoryManagementService)
        {
            _jokeCategoryManagementService = jokeCategoryManagementService;
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditJokeCategory")]

        public JsonResult<ResponseViewModel<bool>> PostEditJokeCategory(JokeCategoryViewModel jokeCategory)
        {
            var response = ServiceExecutor.Execute(() => _jokeCategoryManagementService.PutJokeCategory(jokeCategory));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostJokeCategory")]
        public JsonResult<ResponseViewModel<JokeCategoryViewModel>> PostJokeCategory(JokeCategoryViewModel jokeCategory)
        {
            var response = ServiceExecutor.Execute(() => _jokeCategoryManagementService.InsertJokeCategory(jokeCategory));
            return Json(response);
        }

        [AllowAnonymous]
        [Route("GetJokeCategories")]
        public JsonResult<ResponseViewModel<JokeCategoryViewModel[]>> GetJokeCategories(int languageId)
        {
            var response = ServiceExecutor.Execute(() => _jokeCategoryManagementService.GetJokeCategories(languageId));
            return Json(response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetJokeCategoryTranslation")]
        public JsonResult<ResponseViewModel<JokeCategoryViewModel>> GetJokeCategoryTranslation(int jokeCategoryId, int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => _jokeCategoryManagementService.GetJokeCategoryTranslation(jokeCategoryId, languageId));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostDeleteJokeCategory")]
        public JsonResult<ResponseViewModel<bool>> PostDeleteJokeCategory(JokeCategoryViewModel jokeCategory)
        {
            var response = ServiceExecutor.Execute(() => _jokeCategoryManagementService.RemoveJokeCategory(jokeCategory));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("GetJokeCategoriesWithAvailableLanguage")]
        public JsonResult<ResponseViewModel<JokeCategoryViewModel[]>> GetJokeCategoriesWithAvailableLanguage(int languageId)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _jokeCategoryManagementService.GetJokeCategoriesWithAvailableLanguage(languageId));
            return Json(response);
        }
    }
}
