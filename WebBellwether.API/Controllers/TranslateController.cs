using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
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
        [Route("PostLanguageTranslation")]
        public async Task<IHttpActionResult> PostLanguageTranslation(TranslateLanguageModel languageModel)
        {
            var resultStateContainer = await _service.GetLanguageTranslation(new TranslateLanguageModel(languageModel.CurrentLanguageCode, languageModel.TargetLanguageCode, languageModel.ContentForTranslation));
            return resultStateContainer.ResultState == ResultState.Success
                ? Ok(resultStateContainer.ResultValue)
                : (IHttpActionResult)BadRequest(resultStateContainer.ResultValue.ToString());
        }
        [Authorize]
        [Route("PostTranslateAllLanguageKeys")]
        public async Task<IHttpActionResult> PostTranslateAllLanguageKeys(TranslateLanguageKeysModel translateLangaugeKeysModel)
        {
            IEnumerable<string> currentLangaugeFile = _languageService.GetLanguageFileValue(translateLangaugeKeysModel.CurrentLanguageId);
            if (currentLangaugeFile == null)
                return BadRequest(ResultMessage.LanguageFileNotExists.ToString());

            var translateResultState = await _service.GetAllLanguageKeysTranslations(new TranslateLanguageModel(translateLangaugeKeysModel.CurrentLanguageShortName,translateLangaugeKeysModel.TargetLangaugeShortName,currentLangaugeFile));
            if (translateResultState.ResultState == ResultState.Failure)
                return BadRequest(translateResultState.ResultValue.ToString());

            var fillLangaugeFileResultState = _languageService.FillLanguageFile(translateResultState.ResultValue as IEnumerable<string>,
                translateLangaugeKeysModel.TargetLanguageId);

            return fillLangaugeFileResultState.ResultState == ResultState.Success
                ? Ok(fillLangaugeFileResultState.ResultMessage)
                : (IHttpActionResult)BadRequest(fillLangaugeFileResultState.ResultValue.ToString());
        }
    }
}
//To step into properties, go to Tools->Options->Debugging and uncheck 'Step over properties and operators (Managed only)'.
