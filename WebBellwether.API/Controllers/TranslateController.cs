using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebBellwether.API.Models.Translation;
using WebBellwether.API.Services.TranslateService;
using WebBellwether.API.Services.TranslateService.Abstract;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/Translate")]
    public class TranslateController : ApiController
    {
        private YandexTranslateService _service;
        public TranslateController()
        {
            _service = new YandexTranslateService();
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
    }
}
