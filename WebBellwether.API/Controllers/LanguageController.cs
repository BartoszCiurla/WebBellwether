using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBellwether.API.Context;
using WebBellwether.API.Models.Translation;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Language")]
    public class LanguageController : ApiController
    {
        private EfDbContext _ctx = new EfDbContext();
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_ctx.Languages.ToList());
        }  
        [Authorize]
        [Route("PostEditLanguageKey")]
        public IHttpActionResult PostEditLanguageKey(LanguageKeyModel languageKey)
        {
            //test 
            //@".\..\\..\..\RssDialogys.Domain\RssImages
            //string json = File.ReadAllText(".../WebBellwether.Web/appData/translations/translation_" + languageKey.LanguageId + ".json");
            //string myfile = @"~\WebBellwether.Web\appdata\translations\translation_" + languageKey.LanguageId + ".json";
            //to w zasadzie działa teraz tylko trzeba repo napisac w sumie to potrzebuje tylko zrobić dodawanie/edytowanie które już w sumie jest + obsluga w ui
            string myfile = @"E:\PRACA INŻYNIERSKA\WebBelwether New\WebBellwether\WebBellwether.Web\appData\translations\translation_" + languageKey.LanguageId + ".json";
            string json = File.ReadAllText(myfile);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj[languageKey.Key] = languageKey.Value;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(myfile, output);
            return Ok();
        }

    }
}
