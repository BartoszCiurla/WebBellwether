using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.Version;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Versions")]
    public class VersionsController : ApiController
    {
        [AllowAnonymous]
        [Route("")]
        public JsonResult<ResponseViewModel<ClientVersionViewModel>> Get(int languageId)
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.VersionService.GetVersion(languageId));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("GetVersionDetailsForLanguage")]
        public JsonResult<ResponseViewModel<VersionAggregateViewModel>> GetVersionDataForLanguage(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.VersionService.GetVersionDetailsForLanguage(languageId));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostNewVersion")]
        public JsonResult<ResponseViewModel<bool>> PostNewVersion(VersionViewModel newVersion)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.VersionService.ChooseTargetAndFunction(newVersion, true));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostRemoveVersion")]
        public JsonResult<ResponseViewModel<bool>> PostRemoveVersion(VersionViewModel versionForDelete)
        {
            var response =
                ServiceExecutor.Execute(
                    () => ServiceFactory.VersionService.ChooseTargetAndFunction(versionForDelete, false));
            return Json(response);
        }
    }
}
