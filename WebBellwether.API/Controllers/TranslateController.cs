using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;
using WebBellwether.API.Utility;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Models.ViewModels;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Translate")]
    public class TranslateController : ApiController
    {
        [Authorize(Roles = "Admin")]
        [Route("GetSupportedLanguages")]
        public JsonResult<ResponseViewModel<SupportedLanguage[]>> GetSupportedLanguages()
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.TranslateService.GetListOfSupportedLanguages());
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("GetTranslateServiceName")]
        public JsonResult<ResponseViewModel<string>> GetTranslateServiceName()
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.TranslateService.GetServiceName());
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostLanguageTranslation")]
        public async Task<JsonResult<ResponseViewModel<JObject>>> PostLanguageTranslation(TranslateLanguageModel languageModel)
        {            
            var result = 
                ServiceExecutor.ExecuteAsync(
                    () =>
                        ServiceFactory.TranslateService.GetLanguageTranslation(
                            new TranslateLanguageModel(languageModel.CurrentLanguageCode,
                                languageModel.TargetLanguageCode, languageModel.ContentForTranslation)));
            return Json(await result);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostTranslateAllLanguageKeys")]
        public async Task<JsonResult<ResponseViewModel<bool>>> PostTranslateAllLanguageKeys(TranslateLanguageKeysModel translateLangaugeKeysModel)
        {
            var valuesToTranslate = ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.GetLanguageFileValue(translateLangaugeKeysModel.CurrentLanguageId));
            if (!valuesToTranslate.IsValid)
                return Json(new ResponseViewModel<bool> { IsValid = false, ErrorMessage = ThrowMessage.LanguageFileNotExists.ToString() });

            var valuesAfterTranslation = await ServiceExecutor.ExecuteAsync(
                        () =>
                            ServiceFactory.TranslateService.GetAllLanguageKeysTranslations(
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
                    () => ServiceFactory.ManagementLanguageService.FillLanguageFile(valuesAfterTranslation.Data, translateLangaugeKeysModel.TargetLanguageId));
            return Json(valuesSaved);
        }
    }
}

