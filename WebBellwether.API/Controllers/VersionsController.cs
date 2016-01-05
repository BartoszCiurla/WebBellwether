using System.Web.Http;
using WebBellwether.Models.Models.Version;
using WebBellwether.Services.Services.VersionService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Versions")]
    public class VersionsController : ApiController
    {
        private readonly IVersionService _service;
        public VersionsController()
        {
            _service = new VersionService();
        }
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult Get(int languageId)
        {
            return Ok(_service.GetVersion(languageId));
        }
        [Authorize(Roles = "Admin")]
        [Route("GetVersionDetailsForLanguage")]
        public IHttpActionResult GetVersionDataForLanguage(int languageId)
        {
            return Ok(_service.GetVersionDetailsForLanguage(languageId));
        }

        [Authorize(Roles = "Admin")]
        [Route("PostNewVersion")]
        public IHttpActionResult PostNewVersion(VersionModel newVersion)
        {
            return Ok(_service.ChooseTargetAndFunction(newVersion,true));
        }

        [Authorize(Roles = "Admin")]
        [Route("PostRemoveVersion")]
        public IHttpActionResult PostRemoveVersion(VersionModel versionForDelete)
        {
            return Ok(_service.ChooseTargetAndFunction(versionForDelete,false));
        }
    }
}
