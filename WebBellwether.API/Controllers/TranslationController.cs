using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBellwether.API.Models.Translation;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Translation")]
    public class TranslationController : ApiController
    {


//        https://translate.yandex.net/api/v1.5/tr.json/getLangs ? 
//key=<API key>
// & [ui=<language code>]
// & [callback=<name of the callback function>]


        //private List<TranslationWebServiceModel> availableServices = new List<TranslationWebServiceModel>
        //{
        //    new TranslationWebServiceModel {ServiceName = "Yandex",ApiUrl = "https://translate.yandex.net/api/v1.5/tr.json/translate?",UseApiKey =true,ApiKeyTemplate="key",LanguageTemplate="lang",TextInputTemplate="text",IsPrimary = true }
        //};

        //[Authorize]
        //[Route("GetAvailableWebServices")]
        //public IHttpActionResult GetAvailableWebServices()
        //{
        //    //return Ok(availableServices);
        //    return Ok(new TranslationWebServiceModel { ServiceName = "Yandex", ApiUrl = "https://translate.yandex.net/api/v1.5/tr.json/translate?", UseApiKey = true, ApiKeyTemplate = "key", LanguageTemplate = "lang", TextInputTemplate = "text", IsPrimary = true });
        //}

        //var superTest = 'https://translate.yandex.net/api/v1.5/tr.json/translate?key=' + 'trnsl.1.1.20151017T111637Z.54c56d436735854a.e8642bcd77612c2534f47bb494e96fba7fca5c5a' + '&lang=' + currentLangugae + '-' + targetLanguage + '&text=' + header + '&text=' + content;
        //        return $http.get(superTest).then(function (x)
        //{
        //    return x;


        [Authorize]
        [Route("GetOuterWebServiceKey")]
        public IHttpActionResult GetOuterWebServiceKey(string serviceName)
        {
            //this is temporary solution ... 
            Dictionary<string, string> BaseKeys = new Dictionary<string, string>
            {
                {"Yandex","trnsl.1.1.20151017T111637Z.54c56d436735854a.e8642bcd77612c2534f47bb494e96fba7fca5c5a"}
            };
            string result = BaseKeys[serviceName];
            if (result == null)
                return BadRequest();
            else
                return Ok(result);
        }
    }
}
