using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Models.Translation.Yandex;
using WebBellwether.Models.Results;
using WebBellwether.Services.Services.TranslateService.Abstract;

namespace WebBellwether.Services.Services.TranslateService
{
    public class YandexTranslateService : ITranslateService
    {
        public static string ApiKey { get; } =
            "key=trnsl.1.1.20151017T111637Z.54c56d436735854a.e8642bcd77612c2534f47bb494e96fba7fca5c5a";

        public static string BaseAdress { get; } = "https://translate.yandex.net/api/v1.5/tr.json/";
        public static string Translate { get; } = "translate?";

        // 401 invalid api key
        // 402 blocked api key
        // 403 Exceeded the daily limit on the number of requests
        // 404 Exceeded the daily limit on the amount of translated text
        // 413 Exceeded the maximum text size
        // 422 The text cannot be translated
        // 501 The specified translation direction is not supported
        //max 10k characters 
        //max size request = 10kb

        private readonly HttpClient _webClient;

        public YandexTranslateService()
        {
            _webClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
        }
        public List<SupportedLanguage> GetListOfSupportedLanguages()
        {
            var resultDict = new Dictionary<string, string>
            {
            {"Albanian", "sq"},
            {"English", "en"},
            {"Arabic", "ar"},
            {"Armenian", "hy"},
            {"Azerbaijan", "az"},
            {"Afrikaans", "af"},
            {"Basque", "eu"},
            {"Belarusian", "be"},
            {"Bulgarian", "bg"},
            {"Bosnian", "bs"},
            {"Welsh", "cy"},
            {"Vietnamese", "vi"},
            {"Hungarian", "hu"},
            {"Haitian (Creole)", "ht"},
            {"Galician", "gl"},
            {"Dutch", "nl"},
            {"Greek", "el"},
            {"Georgian", "ka"},
            {"Danish", "da"},
            {"Yiddish", "he"},
            {"Indonesian", "id"},
            {"Irish", "ga"},
            {"Italian", "it"},
            {"Icelandic", "is"},
            {"Spanish", "es"},
            {"Kazakh", "kk"},
            {"Catalan", "ca"},
            {"Kyrgyz", "ky"},
            {"Chinese", "zh"},
            {"Korean", "ko"},
            {"Latin", "la"},
            {"Latvian", "lv"},
            {"Lithuanian", "lt"},
            {"Malagasy", "mg"},
            {"Malay", "ms"},
            {"Maltese", "mt"},
            {"Macedonian", "mk"},
            {"Mongolian", "mn"},
            {"German", "de"},
            {"Norwegian", "no"},
            {"Persian", "fa"},
            {"Polish", "pl"},
            {"Portuguese", "pt"},
            {"Romanian", "ro"},
            {"Russian", "ru"},
            {"Serbian", "sr"},
            {"Slovakian", "sk"},
            {"Slovenian", "sl"},
            {"Swahili", "sw"},
            {"Tajik", "tg"},
            {"Thai", "th"},
            {"Tagalog", "tl"},
            {"Tatar", "tt"},
            {"Turkish", "tr"},
            {"Uzbek", "uz"},
            {"Ukrainian", "uk"},
            {"Finish", "fi"},
            {"French", "fr"},
            {"Croatian", "hr"},
            {"Czech", "cs"},
            {"Swedish", "sv"},
            {"Estonian", "et"},
            {"Japanese", "ja"}
            };
            return resultDict.Select(item => new SupportedLanguage(item.Key, item.Value)).ToList();
        }

        public string GetServiceName()
        {
            return "Yandex";
        }

        public async Task<ResultStateContainer> GetLanguageTranslation(TranslateLanguageModel languageModel)
        {
            try
            {
                string requestString =
                    $"{BaseAdress}{Translate}{ApiKey}{"&lang="}{languageModel.CurrentLanguageCode}{"-"}{languageModel.TargetLanguageCode}";
                var requestContent = new StringBuilder();
                languageModel.ContentForTranslation.ToList().ForEach(x =>
                {
                    requestContent.Append("&text=");
                    requestContent.Append(x);
                });
                var result = await _webClient.GetStringAsync($"{requestString}{requestContent}").ConfigureAwait(false);
                return new ResultStateContainer { ResultState = ResultState.Success, ResultValue = JObject.Parse(result) };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultValue = e };
            }

        }

        public async Task<ResultStateContainer> GetAllLanguageKeysTranslations(TranslateLanguageModel languageModel)
        {
            try
            {
                var baseRequestParameters = $"{BaseAdress}{Translate}{ApiKey}{"&lang="}{languageModel.CurrentLanguageCode}{"-"}{languageModel.TargetLanguageCode}";
                var valuesForTranslate = new StringBuilder();
                languageModel.ContentForTranslation.ToList().ForEach(x =>
                {
                    valuesForTranslate.Append("&text=");
                    valuesForTranslate.Append(x);
                });
                //w przyszłości to nie będzie działało ... i bede musial to robic po trochu ... obecnie mam około 7k + znaków ...
                if (valuesForTranslate.ToString().Count() > 10000)
                    return new ResultStateContainer
                    {
                        ResultState = ResultState.Failure,
                        ResultValue = ResultMessage.TooLongRequest
                    };
                var resultRequest = await _webClient.GetStringAsync($"{baseRequestParameters}{valuesForTranslate}").ConfigureAwait(false);
                YandexResponse result = JsonConvert.DeserializeObject<YandexResponse>(resultRequest);                         

                return new ResultStateContainer { ResultState = ResultState.Success, ResultValue = result.Text };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultValue = e };
            }
        }
    }
}