using System.Web.Http;
using WebBellwether.API.Models.Translation;
using WebBellwether.API.Services.LanguageService;
using WebBellwether.API.Results;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Repositories;
using WebBellwether.API.Services.LanguageService.Abstract;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Language")]
    public class LanguageController : ApiController
    {
        private readonly IManagementLanguageService _service;
        public LanguageController()
        {
            //this route is fs must change it ...
            _service = new ManagementLanguageService(new AggregateRepositories(), @"E:\PRACA INŻYNIERSKA\WebBelwether New\WebBellwether\WebBellwether.Web\appData\translations\translation_");
        }
        
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_service.GetLanguages());
        }  
        
        [Route("GetAllLanguages")]
        public IHttpActionResult GetAllLanguages()
        {
            return Ok(_service.GetLanguages(true));
        }

        [AllowAnonymous]
        [Route("GetLanguage")]
        public IHttpActionResult GetLanguage(int languageId)
        {
            return Ok(_service.GetLanguageById(languageId));
        }

        [AllowAnonymous]
        [Route("GetLanguageFile")]
        public IHttpActionResult GetLanguageFile(int languageId)
        {
            return Ok(_service.GetLanguageFile(languageId));
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditLanguageKey")]
        public IHttpActionResult PostEditLanguageKey(LanguageKeyModel languageKey)
        {
            ResultStateContainer result = _service.PutLanguageKey(languageKey);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());         
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditLanguage")]
        public IHttpActionResult PostEditLangauge(Language language)
        {
            ResultStateContainer result = _service.PutLanguage(language);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }
        [Authorize(Roles = "Admin")]
        [Route("PostPublishLanguage")]
        public IHttpActionResult PostPublishLanguage(Language language)
        {
            ResultStateContainer result = _service.PublishLanguage(language);
            return result.ResultState == ResultState.Success ? Ok(result.ResultMessage.ToString()) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }
        [Authorize(Roles = "Admin")]
        [Route("PostLanguage")]
        public IHttpActionResult PostLanguage(Language language)
        {
            ResultStateContainer result = _service.PostLanguage(language);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }

        [Authorize(Roles = "Admin")]
        [Route("PostDeleteLanguage")]
        public IHttpActionResult PostDeleteLanguage(Language language)
        {
            ResultStateContainer result = _service.DeleteLanguage(language);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }

    }
}
