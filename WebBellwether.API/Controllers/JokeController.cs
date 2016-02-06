using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.Joke;
using WebBellwether.Services.Services.JokeService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Jokes")]
    public class JokeController : ApiController
    {
        private readonly IJokeService _jokeService;

        public JokeController(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }
        [AllowAnonymous]
        [Route("GetJokes")]
        public JsonResult<ResponseViewModel<JokeViewModel[]>> GetJokes(int languageId)
        {
            var response = ServiceExecutor.Execute(() => _jokeService.GetJokes(languageId));
            return Json(response);
        }              
    }
}
