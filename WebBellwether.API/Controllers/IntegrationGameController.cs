using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.IntegrationGame;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/IntegrationGame")]
    public class IntegrationGameController : ApiController
    {
        [AllowAnonymous]
        [Route("GetIntegrationGames")]
        public JsonResult<ResponseViewModel<DirectIntegrationGameViewModel[]>> GetIntegrationGames(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.IntegrationGameService.GetIntegrationGames(languageId));
            return Json(response);
        }  
    }
}
