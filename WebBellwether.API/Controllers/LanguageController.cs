using System.Web.Http;
using WebBellwether.API.Context;
using WebBellwether.API.Models.Translation;
using WebBellwether.API.Services.LanguageService;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Results;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Language")]
    public class LanguageController : ApiController
    {
        private EfDbContext _ctx = new EfDbContext();
        private ManagementLanguageService _service;
        public LanguageController()
        {
            _service = new ManagementLanguageService(new LanguageUnitOfWork(), @"E:\PRACA INŻYNIERSKA\WebBelwether New\WebBellwether\WebBellwether.Web\appData\translations\translation_");
        }
        
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_service.GetLanguages());
        }  
        [Authorize]
        [Route("PostEditLanguageKey")]
        public IHttpActionResult PostEditLanguageKey(LanguageKeyModel languageKey)
        {
            ResultStateContainer result = _service.PutLanguageKey(languageKey);
            switch (result.ResultState)
            {
                case ResultState.LanguageKeyValueEdited:
                    return Ok();
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CriticalError");
            }           
        }

    }
}
