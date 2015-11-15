using System.Web.Http;
using WebBellwether.API.Context;
using WebBellwether.API.Models.Translation;
using WebBellwether.API.Services.LanguageService;
using WebBellwether.API.Results;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Repositories;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Language")]
    public class LanguageController : ApiController
    {
        private ManagementLanguageService _service;
        public LanguageController()
        {
            _service = new ManagementLanguageService(new AggregateRepositories(), @"E:\PRACA INŻYNIERSKA\WebBelwether New\WebBellwether\WebBellwether.Web\appData\translations\translation_");
        }
        
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_service.GetLanguages());
        }  
        [Authorize]
        [Route("GetAllLanguages")]
        public IHttpActionResult GetAllLanguages()
        {
            return Ok(_service.GetAllLanguages());
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
            //koniecznie musze się zastanowic jak zrobić to inaczej bo takie rozwiązanie nie podoba mi się 
            //teoretycznie mozna by 
            ResultStateContainer result = _service.PublishLanguage(language);
            switch (result.ResultState)
            {
                case ResultState.LanguageFileNotExists:
                    return BadRequest(result.ResultState.ToString());
                case ResultState.EmptyKeysExists:
                    return BadRequest(result.ResultState.ToString());
                case ResultState.OnlyOnePublicLanguage:
                    return BadRequest(result.ResultState.ToString());
                case ResultState.LanguageNotExists:
                    return BadRequest(result.ResultState.ToString());
                case ResultState.LanguageHasBeenPublished:
                    return Ok(result.ResultState.ToString());
                case ResultState.LanguageHasBeenNonpublic:
                    return Ok(result.ResultState.ToString());
                default:
                    return BadRequest("CriticalError");
            }
        }
        [Authorize]
        [Route("PostLanguage")]
        public IHttpActionResult PostLanguage(Language language)
        {
            ResultStateContainer result = _service.PostLanguage(language);
            return Ok();
        }

    }
}
