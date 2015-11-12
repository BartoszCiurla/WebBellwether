using System.Web.Http;
using WebBellwether.API.Context;
using WebBellwether.API.Models.Translation;
using WebBellwether.API.Services.LanguageService;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Results;
using WebBellwether.API.Entities.Translations;

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
        [Authorize]
        [Route("PostEditLanguage")]
        public IHttpActionResult PostEditLangauge(Language language)
        {
            ResultStateContainer result = _service.PutLanguage(language);
            switch (result.ResultState)
            {
                case ResultState.LanguageEdited:
                    return Ok();
                case ResultState.LanguageNotExists:
                    return BadRequest("LanguageNotExists");
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CriticalError");
            }
        }
        [Authorize]
        [Route("PostPublishLanguage")]
        public IHttpActionResult PostPublishLanguage(Language language)
        {
            ResultStateContainer result = _service.PublishLanguage(language);
            return Ok();
        }

    }
}
