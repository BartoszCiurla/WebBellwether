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
            var postLanguageTranslation = function(translateLanguageModel) {
                return $http.post(serviceBase + "api/Translate/PostLanguageTranslation", translateLanguageModel).then(function (x) {
                    return x;
                });
            };
            var postTranslateAllLanguageKeys = function (translateLangaugeKeysModel) {
                return $http.post(serviceBase + "api/Translate/PostTranslateAllLanguageKeys",translateLangaugeKeysModel).then(function (x) {
                    return x;
                });
            };        

            translateServiceFactory.GetLanguages = getLangs;
            translateServiceFactory.GetTranslateServiceName = getTranslateServiceName;
            translateServiceFactory.PostLanguageTranslation = postLanguageTranslation;
            translateServiceFactory.PostTranslateAllLanguageKeys = postTranslateAllLanguageKeys;
            return translateServiceFactory;
        }]);
})();
