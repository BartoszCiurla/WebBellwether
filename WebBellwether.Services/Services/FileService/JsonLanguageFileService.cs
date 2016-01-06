using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebBellwether.Models.Models.Translation;

namespace WebBellwether.Services.Services.FileService
{
    public interface ILanguageFileService
    {
        IEnumerable<string> GetFileValues(int languageId);
        int GetFileEmptyKeys(int languageId);
        bool FillFile(string[] values, int languageId);
        bool CreateFile(int languageId);
        Dictionary<string, string> GetFile(int languageId);
        bool RemoveFile(int languageId);
        bool PutLanguageKey(LanguageKeyModel languageKey);
    }
    public class JsonLanguageFileService : ILanguageFileService
    {
        private const string FileLocation =
            @"E:\PRACA INŻYNIERSKA\WebBelwether New\WebBellwether\WebBellwether.Web\appData\translations\translation_";
        private const int TemplateLanguageFileNumerator = 1;

        public IEnumerable<string> GetFileValues(int languageId)
        {
            return GetFileAsDictionary(languageId).Select(x => x.Value);
        }

        public int GetFileEmptyKeys(int languageId)
        {
            return GetFileAsDictionary(languageId).Count(x => x.Value.Length < 1);
        }

        public bool FillFile(string[] values, int languageId)
        {
            Dictionary<string, string> languageFile = GetFileAsDictionary(languageId);
            if (!languageFile.Count().Equals(values.Count()))
                return false;
            for (int i = 0; i < languageFile.Count; i++)
            {
                languageFile[languageFile.ElementAt(i).Key] = values.ElementAt(i);
            }
            return WriteFile(JsonConvert.SerializeObject(languageFile), GetFileLocation(languageId));
        }

        public bool CreateFile(int languageId)
        {
            IEnumerable<JProperty> jsonKeys = GetFileKeys().Select(x => new JProperty(x.Key, x.Value)).ToArray();
            JObject jsonJObject = new JObject(jsonKeys);
            using (StreamWriter file = File.CreateText(GetFileLocation(languageId)))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                jsonJObject.WriteTo(writer);
            }
            return true;
        }

        public Dictionary<string,string> GetFile(int languageId)
        {
            //var tempDictJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetFileAsString(languageId));
            //return tempDictJson.ToList().Select(x => new LanguageFilePosition { Key = x.Key, Value = x.Value }).ToArray();
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(GetFileAsString(languageId));
        }

        public bool RemoveFile(int languageId)
        {
            if (!File.Exists(GetFileLocation(languageId)))
                return false;
            File.Delete(GetFileLocation(languageId));
            return true;
        }

        public bool PutLanguageKey(LanguageKeyModel languageKey)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(GetFileAsString(languageKey.LanguageId));
            jsonObj[languageKey.Key] = languageKey.Value;
            string outFile = JsonConvert.SerializeObject(jsonObj);
            return WriteFile(outFile, GetFileLocation(languageKey.LanguageId));
        }

        private string GetFileLocation(int languageId)
        {
            return $"{FileLocation}{languageId}{".json"}";
        }

        private string GetFileAsString(int languageId)
        {
            return File.ReadAllText(GetFileLocation(languageId));
        }

        private bool WriteFile(string jsonFormatFile,string fileLocation)
        {
            File.WriteAllText(fileLocation, jsonFormatFile);
            return true;
        }

        private Dictionary<string, string> GetFileAsDictionary(int languageId)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(GetFileAsString(languageId));
        } 

        private Dictionary<string, string> GetFileKeys()
        {
            return GetFileAsDictionary(TemplateLanguageFileNumerator).ToDictionary(position => position.Key, position => "");
        }     
    }
}
