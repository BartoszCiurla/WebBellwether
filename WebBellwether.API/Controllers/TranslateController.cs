using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Http;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models.Translation;
using WebBellwether.API.Repositories;
using WebBellwether.API.Results;
using WebBellwether.API.Services.LanguageService;
using WebBellwether.API.Services.TranslateService;
using WebBellwether.API.Services.TranslateService.Abstract;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Translate")]
    public class TranslateController : ApiController
    {
        private readonly ITranslateService _service;
        private readonly ManagementLanguageService _languageService;
        public TranslateController()
        {
            _service = new YandexTranslateService();
            _languageService = new ManagementLanguageService(new AggregateRepositories(), @"E:\PRACA INŻYNIERSKA\WebBelwether New\WebBellwether\WebBellwether.Web\appData\translations\translation_");
        }

        [Authorize]
        [Route("GetSupportedLanguages")]
        public IHttpActionResult GetSupportedLanguages()
        {
            return Ok(_service.GetListOfSupportedLanguages());
        }

        [Authorize]
        [Route("GetTranslateServiceName")]
        public IHttpActionResult GetTranslateServiceName()
        {
            return Ok(_service.GetServiceName());
        }

        [Authorize]
        [Route("GetLanguageKeyTranslation")]
        public async Task<IHttpActionResult> GetLanguageKeyTranslation(string currentLanguage, string targetLanguage, string content)
        {
            var resultStateContainer = await _service.GetLanguageKeyTranslation(currentLanguage, targetLanguage, content);
            return resultStateContainer.ResultState == ResultState.Success
                ? Ok(resultStateContainer.ResultValue)
                : (IHttpActionResult)BadRequest(resultStateContainer.ResultValue.ToString());
        }

        [Authorize]
        [Route("GetTranslateAllLanguageKeys")]
        public async Task<IHttpActionResult> GetTranslateAllLanguageKeys(int currentLanguageId, int targetLanguageId, string currentShortName, string targetShortname)
        {
            Dictionary<string, string> currentLangaugeFile = _languageService.GetLanguageFile(currentLanguageId);
            if (currentLangaugeFile == null)
                return BadRequest(ResultMessage.LanguageFileNotExists.ToString());
            var translateResultState = await _service.TranslateAllLanguageKeys(currentLangaugeFile, currentShortName, targetShortname);
            if (translateResultState.ResultState == ResultState.Failure)
                return BadRequest(translateResultState.ResultValue.ToString());
            var fillLangaugeFileResultState = _languageService.FillLanguageFile(translateResultState.ResultValue as List<string>,
                targetLanguageId);

            return fillLangaugeFileResultState.ResultState == ResultState.Success
                ? Ok()
                : (IHttpActionResult)BadRequest(fillLangaugeFileResultState.ResultValue.ToString());
        }
    }
}
//To step into properties, go to Tools->Options->Debugging and uncheck 'Step over properties and operators (Managed only)'.
