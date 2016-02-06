using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.IntegrationGame;
using WebBellwether.Services.Services.IntegrationGameService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/IntegrationGameManagement")]
    public class IntegrationGameManagementController : ApiController
    {
        private readonly IIntegrationGameManagementService _managementIntegrationGamesService;

        public IntegrationGameManagementController(IIntegrationGameManagementService managementIntegrationGamesService)
        {
            _managementIntegrationGamesService = managementIntegrationGamesService;
        }
        [Authorize(Roles = "Admin")]
        [Route("PostIntegrationGame")]
        public JsonResult<ResponseViewModel<IntegrationGameViewModel>> PostIntegrationGame(NewIntegrationGameViewModel game)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _managementIntegrationGamesService.InsertIntegrationGame(game));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("GetIntegrationGamesWithAvailableLanguage")]
        public JsonResult<ResponseViewModel<List<IntegrationGameViewModel>>> GetIntegrationGamesWithAvailableLanguage(int languageId)
        {
            var response =
                ServiceExecutor.Execute(
                    () =>
                        _managementIntegrationGamesService.GetIntegrationGamesWithAvailableLanguages(
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
                    () => _managementIntegrationGamesService.GetGameTranslation(gameId, languageId));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostDeleteIntegrationGame")]
        public JsonResult<ResponseViewModel<bool>> PostDeleteIntegrationGame(IntegrationGameViewModel integrationGame)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _managementIntegrationGamesService.RemoveIntegratiomGame(integrationGame));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditIntegrationGame")]
        public JsonResult<ResponseViewModel<bool>> PostEditIntegrationGame(IntegrationGameViewModel integrationGame)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _managementIntegrationGamesService.PutIntegrationGame(integrationGame));
            return Json(response);
        }     
    }
}
