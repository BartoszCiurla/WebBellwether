using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.ViewModels;
using WebBellwether.Services.Services.LanguageService;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Language")]
    public class LanguageManagementController : ApiController
    {
        private readonly ILanguageManagementService _managementLanguageService;

        public LanguageManagementController(ILanguageManagementService managementLanguageService)
        {
            _managementLanguageService = managementLanguageService;
        }
        [AllowAnonymous]
        [Route("")]
        public JsonResult<ResponseViewModel<Language[]>> Get()
        {
            var response = ServiceExecutor.Execute(() => _managementLanguageService.GetLanguages());
            return Json(response);
        }  
        
        [Route("GetAllLanguages")]
        public JsonResult<ResponseViewModel<Language[]>> GetAllLanguages()
        {
            var response = ServiceExecutor.Execute(() => _managementLanguageService.GetLanguages(true));
            return Json(response);
        }

        [AllowAnonymous]
        [Route("GetLanguage")]
        public JsonResult<ResponseViewModel<Language>> GetLanguage(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => _managementLanguageService.GetLanguageById(languageId));
            return Json(response);
        }

        [AllowAnonymous]
        [Route("GetLanguageFile")]
        public JsonResult<ResponseViewModel<IEnumerable<LanguageFilePosition>>> GetLanguageFile(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => _managementLanguageService.GetLanguageFile(languageId));
            return Json(response);           
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditLanguageKey")]
        public JsonResult<ResponseViewModel<bool>> PostEditLanguageKey(LanguageKeyModel languageKey)
        {
            var response =
                ServiceExecutor.Execute(() => _managementLanguageService.PutLanguageKey(languageKey));
            return Json(response);            
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditLanguage")]
        public JsonResult<ResponseViewModel<bool>> PostEditLangauge(Language language)
        {
            var response = ServiceExecutor.Execute(() => _managementLanguageService.PutLanguage(language));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostPublishLanguage")]
        public JsonResult<ResponseViewModel<string>> PostPublishLanguage(Language language)
        {
            var response =
                ServiceExecutor.Execute(() => _managementLanguageService.PublishLanguage(language));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostLanguage")]
        public JsonResult<ResponseViewModel<Language>> PostLanguage(Language language)
        {
            var response = ServiceExecutor.Execute(() => _managementLanguageService.PostLanguage(language));
            return Json(response);            
        }

        [Authorize(Roles = "Admin")]
        [Route("PostDeleteLanguage")]
        public JsonResult<ResponseViewModel<bool>> PostDeleteLanguage(Language language)
        {
            var response =
                ServiceExecutor.Execute(() => _managementLanguageService.DeleteLanguage(language));
            return Json(response);
        }
    }
}
