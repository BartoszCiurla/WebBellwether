using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.ViewModels;
using WebBellwether.Models.ViewModels.Version;
using WebBellwether.Services.Services.VersionService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Version")]
    public class VersionController : ApiController
    {
        private readonly IVersionService _versionService;

        public VersionController(IVersionService versionService)
        {
            _versionService = versionService;
        }
        [AllowAnonymous]
        [Route("")]
        public JsonResult<ResponseViewModel<ClientVersionViewModel>> Get(int languageId)
        {
            var response = ServiceExecutor.Execute(() => _versionService.GetVersion(languageId));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("GetVersionDetailsForLanguage")]
        public JsonResult<ResponseViewModel<VersionAggregateViewModel>> GetVersionDataForLanguage(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => _versionService.GetVersionDetailsForLanguage(languageId));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostNewVersion")]
        public JsonResult<ResponseViewModel<bool>> PostNewVersion(VersionViewModel newVersion)
        {
            var response =
                ServiceExecutor.Execute(() => _versionService.ChooseTargetAndFunction(newVersion, true));
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostRemoveVersion")]
        public JsonResult<ResponseViewModel<bool>> PostRemoveVersion(VersionViewModel versionForDelete)
        {
            var response =
                ServiceExecutor.Execute(
                    () => _versionService.ChooseTargetAndFunction(versionForDelete, false));
            return Json(response);
        }
    }
}
