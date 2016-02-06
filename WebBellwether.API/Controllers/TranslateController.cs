using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;
using WebBellwether.API.Utility;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Models.ViewModels;
using WebBellwether.Services.Services.LanguageService;
using WebBellwether.Services.Services.TranslateService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Translate")]
    public class TranslateController : ApiController
    {
        private readonly ITranslateService _translateService;
        private readonly ILanguageManagementService _managementLanguageService;

        public TranslateController(ITranslateService translateService,ILanguageManagementService managementLanguageService)
        {
            _translateService = translateService;
            _managementLanguageService = managementLanguageService;
        }
        [Authorize(Roles = "Admin")]
        [Route("GetSupportedLanguages")]
        public JsonResult<ResponseViewModel<SupportedLanguage[]>> GetSupportedLanguages()
        {
            var response = ServiceExecutor.Execute(() => _translateService.GetListOfSupportedLanguages());
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("GetTranslateServiceName")]
        public JsonResult<ResponseViewModel<string>> GetTranslateServiceName()
        {
            var response = ServiceExecutor.Execute(() => _translateService.GetServiceName());
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostLanguageTranslation")]
        public async Task<JsonResult<ResponseViewModel<JObject>>> PostLanguageTranslation(TranslateLanguageModel languageModel)
        {            
            var result = 
                ServiceExecutor.ExecuteAsync(
                    () =>
                        _translateService.GetLanguageTranslation(
                            new TranslateLanguageModel(languageModel.CurrentLanguageCode,
                                languageModel.TargetLanguageCode, languageModel.ContentForTranslation)));
            return Json(await result);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostTranslateAllLanguageKeys")]
        public async Task<JsonResult<ResponseViewModel<bool>>> PostTranslateAllLanguageKeys(TranslateLanguageKeysModel translateLangaugeKeysModel)
        {
            var valuesToTranslate = ServiceExecutor.Execute(() => _managementLanguageService.GetLanguageFileValue(translateLangaugeKeysModel.CurrentLanguageId));
            if (!valuesToTranslate.IsValid)
                return Json(new ResponseViewModel<bool> { IsValid = false, ErrorMessage = ThrowMessage.LanguageFileNotExists.ToString() });

            var valuesAfterTranslation = await ServiceExecutor.ExecuteAsync(
                        () =>
                            _translateService.GetAllLanguageKeysTranslations(
                                new TranslateLanguageModel(translateLangaugeKeysModel.CurrentLanguageShortName,
                                    translateLangaugeKeysModel.TargetLangaugeShortName,
                                    valuesToTranslate.Data.ToArray())));

            if(!valuesAfterTranslation.IsValid)
                return
                    Json(new ResponseViewModel<bool>
                    {
                        IsValid = false,
                        ErrorMessage = valuesAfterTranslation.ErrorMessage
                    });
            var valuesSaved =
                ServiceExecutor.Execute(
                    () => _managementLanguageService.FillLanguageFile(valuesAfterTranslation.Data, translateLangaugeKeysModel.TargetLanguageId));
            return Json(valuesSaved);
        }
    }
}

