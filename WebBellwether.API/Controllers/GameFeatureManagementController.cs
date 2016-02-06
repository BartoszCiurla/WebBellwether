using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.IntegrationGame;
using WebBellwether.Services.Services.IntegrationGameService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/GameFeatureManagement")]
    public class GameFeatureManagementController : ApiController
    {
        private readonly IGameFeatureManagementService _gameFeatureManagementService;

        public GameFeatureManagementController(IGameFeatureManagementService gameFeatureManagementService)
        {
            _gameFeatureManagementService = gameFeatureManagementService;
        }
        [AllowAnonymous]
        [Route("GetGameFeatureDetails")]
        public JsonResult<ResponseViewModel<GameFeatureDetailViewModel[]>> GetGameFeatureDetails(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => _gameFeatureManagementService.GetGameFeatureDetails(languageId));
            return Json(response);
        }

        [AllowAnonymous]
        [Route("GetGameFeatuesModelWithDetails")]
        public JsonResult<ResponseViewModel<GameFeatureViewModel[]>> GetGameFeatuesModelWithDetails(int languageId)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _gameFeatureManagementService.GetGameFeatuesModelWithDetails(languageId));
            return Json(response);
        }

        [AllowAnonymous]
        [Route("GetGameFeatures")]
        public JsonResult<ResponseViewModel<GameFeatureViewModel[]>> GetGameFeatures(int languageId)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _gameFeatureManagementService.GetGameFeatures(languageId));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostGameFeature")]
        public JsonResult<ResponseViewModel<bool>> PostGameFeature(GameFeatureViewModel gameFeature)
        {
            var response =
                ServiceExecutor.Execute(() => _gameFeatureManagementService.PutGameFeature(gameFeature));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostGameFeatureDetail")]
        public JsonResult<ResponseViewModel<bool>> PostGameFeatureDetail(GameFeatureDetailViewModel gameFeatureDetail)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _gameFeatureManagementService.PutGameFeatureDetail(gameFeatureDetail));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostCreateGameFeatures")]
        public JsonResult<ResponseViewModel<GameFeatureViewModel[]>> PostCreateGameFeatures(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => _gameFeatureManagementService.CreateGameFeatures(languageId));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostGameFeatures")]
        public JsonResult<ResponseViewModel<bool>> PostGameFeatures(GameFeatureViewModel[] gameFeatures)
        {
            var response =
                ServiceExecutor.Execute(() => _gameFeatureManagementService.PutGameFeatures(gameFeatures));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostGameFeatureDetails")]
        public JsonResult<ResponseViewModel<bool>> PostGameFeatureDetails(GameFeatureDetailViewModel[] gameFeatureDetails)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _gameFeatureManagementService.PutGameFeatureDetails(gameFeatureDetails));
            return Json(response);
        }
    }
}
