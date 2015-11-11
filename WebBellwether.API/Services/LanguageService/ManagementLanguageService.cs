using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Results;
using WebBellwether.API.Models.Translation;
using System.IO;

namespace WebBellwether.API.Services.LanguageService
{
    public class ManagementLanguageService
    {
        private LanguageUnitOfWork _unitOfWork;
        private string destinationPlace;
        public ManagementLanguageService(LanguageUnitOfWork unitOfWork,string destPlace)
        {
            _unitOfWork = unitOfWork;
            destinationPlace = destPlace;
        } 
        public List<Language> GetLanguages()
        {
            return _unitOfWork.LanguageRepository.Get().ToList();
        }
        public ResultStateContainer PutLanguageKey(LanguageKeyModel languageKey)
        {
            try
            {
                string jsonLocation = destinationPlace + languageKey.LanguageId + ".json";
                string json = File.ReadAllText(jsonLocation);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj[languageKey.Key] = languageKey.Value;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonLocation, output);
                return new ResultStateContainer { ResultState = ResultState.LanguageKeyValueEdited };
            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error,Value = e };
            }
        }
    }
}
