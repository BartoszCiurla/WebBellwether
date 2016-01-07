using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using WebBellwether.API.Utility;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.ViewModels;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Language")]
    public class LanguageController : ApiController
    {        
        [AllowAnonymous]
        [Route("")]
        public JsonResult<ResponseViewModel<Language[]>> Get()
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.GetLanguages());
            return Json(response);
        }  
        
        [Route("GetAllLanguages")]
        public JsonResult<ResponseViewModel<Language[]>> GetAllLanguages()
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.GetLanguages(true));
            return Json(response);
        }

        [AllowAnonymous]
        [Route("GetLanguage")]
        public JsonResult<ResponseViewModel<Language>> GetLanguage(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.GetLanguageById(languageId));
            return Json(response);
        }

        [AllowAnonymous]
        [Route("GetLanguageFile")]
        public JsonResult<ResponseViewModel<IEnumerable<LanguageFilePosition>>> GetLanguageFile(int languageId)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.GetLanguageFile(languageId));
            return Json(response);           
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditLanguageKey")]
        public JsonResult<ResponseViewModel<bool>> PostEditLanguageKey(LanguageKeyModel languageKey)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.PutLanguageKey(languageKey));
            return Json(response);            
        }
        [Authorize(Roles = "Admin")]
        [Route("PostEditLanguage")]
        public JsonResult<ResponseViewModel<bool>> PostEditLangauge(Language language)
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.PutLanguage(language));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostPublishLanguage")]
        public JsonResult<ResponseViewModel<string>> PostPublishLanguage(Language language)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.PublishLanguage(language));
            return Json(response);
        }
        [Authorize(Roles = "Admin")]
        [Route("PostLanguage")]
        public JsonResult<ResponseViewModel<int>> PostLanguage(Language language)
        {
            var response = ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.PostLanguage(language));
            return Json(response);            
        }

        [Authorize(Roles = "Admin")]
        [Route("PostDeleteLanguage")]
        public JsonResult<ResponseViewModel<bool>> PostDeleteLanguage(Language language)
        {
            var response =
                ServiceExecutor.Execute(() => ServiceFactory.ManagementLanguageService.DeleteLanguage(language));
            return Json(response);
        }
    }
}
