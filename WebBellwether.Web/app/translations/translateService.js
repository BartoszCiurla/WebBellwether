(function () {
    angular
        .module('webBellwether')
        .factory('translateService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;
            var translateServiceFactory = {};

            var getLangs = function() {
                return $http.get(serviceBase + "api/Translate/GetSupportedLanguages/").then(function (x) {
                    return x;
                });
            };
            var getTranslateServiceName = function() {
                return $http.get(serviceBase + "api/Translate/GetTranslateServiceName/").then(function(x) {
                    return x;
                });
            };
            var getLanguageKeyTranslation = function(currentLang, targetLang, contentLang) {
                return $http.get(serviceBase + "api/Translate/GetLanguageKeyTranslation/", { params: { currentLanguage: currentLang, targetLanguage: targetLang, content: contentLang } }).then(function (x) {
                    return x;
                });
            };
            var getTranslateAllLanguageKeys = function(currentLang, targetLang) {
                return $http.get(serviceBase + "api/Translate/GetTranslateAllLanguageKeys/",
                    { params: { currentLanguageId: currentLang.id, targetLanguageId: targetLang.id, currentShortName: currentLang.languageShortName, targetShortname: targetLang.languageShortName } }).then(function (x) {
                    return x;
                });
            };
        

            var translateIntegrationGame = function (currentLangugae, targetLanguage, header, content) {
                var superTest = 'https://translate.yandex.net/api/v1.5/tr.json/translate?key=' + 'trnsl.1.1.20151017T111637Z.54c56d436735854a.e8642bcd77612c2534f47bb494e96fba7fca5c5a' + '&lang='+ currentLangugae + '-' + targetLanguage + '&text=' + header + '&text=' + content;
                return $http.get(superTest).then(function (x) {
                    return x;
                });
             
            };

            translateServiceFactory.TranslateIntegrationGame = translateIntegrationGame;
            translateServiceFactory.GetLanguages = getLangs;
            translateServiceFactory.GetTranslateServiceName = getTranslateServiceName;
            translateServiceFactory.GetLanguageKeyTranslation = getLanguageKeyTranslation;
            translateServiceFactory.GetTranslateAllLanguageKeys = getTranslateAllLanguageKeys;
            return translateServiceFactory;
        }]);
})();
