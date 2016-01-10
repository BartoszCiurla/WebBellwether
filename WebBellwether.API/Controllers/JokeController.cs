using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.Joke;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Jokes")]
    public class JokeController : ApiController
    {
        [AllowAnonymous]
        [Route("GetJokes")]
        public JsonResult<ResponseViewModel<JokeViewModel[]>> GetJokes(int languageId)
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.JokeService.GetJokes(languageId));
            return Json(response);
        }              
    }
}
