using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.IntegrationGame;
using WebBellwether.Services.Services.IntegrationGameService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/IntegrationGame")]
    public class IntegrationGameController : ApiController
    {
        private readonly IIntegrationGameService _integrationGameService;

        public IntegrationGameController(IIntegrationGameService integrationGameService)
        {
            _integrationGameService = integrationGameService;
        }
        [AllowAnonymous]
        [Route("GetIntegrationGames")]
        public JsonResult<ResponseViewModel<DirectIntegrationGameViewModel[]>> GetIntegrationGames(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => _integrationGameService.GetIntegrationGames(languageId));
            return Json(response);
        }

        [AllowAnonymous]
        [Route("GetSimpleIntegrationGames")]
        public JsonResult<ResponseViewModel<SimpleIntegrationGameViewModel[]>> GetSimpleIntegrationGames(int languageId)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _integrationGameService.GetSimpleIntegrationGames(languageId));
            return Json(response);
        }
    }
}
