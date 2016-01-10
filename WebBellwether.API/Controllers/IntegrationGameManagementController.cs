using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.IntegrationGame;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/IntegrationGameManagement")]
    public class IntegrationGameManagementController : ApiController
    {
        [Authorize(Roles = "Admin")]
        [Route("PostIntegrationGame")]
        public JsonResult<ResponseViewModel<IntegrationGameViewModel>> PostIntegrationGame(NewIntegrationGameViewModel game)
        {
            var response =
                ServiceExecutor.Execute(
                    () => ServiceFactory.ManagementIntegrationGamesService.InsertIntegrationGame(game));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("GetIntegrationGamesWithAvailableLanguage")]
        public JsonResult<ResponseViewModel<List<IntegrationGameViewModel>>> GetIntegrationGamesWithAvailableLanguage(int languageId)
        {
            var response =
                ServiceExecutor.Execute(
                    () =>
                        ServiceFactory.ManagementIntegrationGamesService.GetIntegrationGamesWithAvailableLanguages(
                            languageId));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetIntegrationGameTranslation")]
        public JsonResult<ResponseViewModel<IntegrationGameViewModel>> GetIntegrationGameTranslation(int gameId, int languageId)
        {
            var response =
                ServiceExecutor.Execute(
                    () => ServiceFactory.ManagementIntegrationGamesService.GetGameTranslation(gameId, languageId));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostDeleteIntegrationGame")]
        public JsonResult<ResponseViewModel<bool>> PostDeleteIntegrationGame(IntegrationGameViewModel integrationGame)
        {
            var response =
                ServiceExecutor.Execute(
                    () => ServiceFactory.ManagementIntegrationGamesService.RemoveIntegratiomGame(integrationGame));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditIntegrationGame")]
        public JsonResult<ResponseViewModel<bool>> PostEditIntegrationGame(IntegrationGameViewModel integrationGame)
        {
            var response =
                ServiceExecutor.Execute(
                    () => ServiceFactory.ManagementIntegrationGamesService.PutIntegrationGame(integrationGame));
            return Json(response);
        }     
    }
}
