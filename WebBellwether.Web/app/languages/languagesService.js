(function () {
    angular
        .module('webBellwether')
        .factory('languagesService', ['$http', '$resource', 'ngAuthSettings', function ($http, $resource, ngAuthSettings) {
            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var getLanguageContent = function (lang) {
                var languageFilePath = '/appData/translations/translation_' + lang + '.json';
                var myResource = $resource(languageFilePath).get();
                return myResource.$promise.then(function (data) {
                    return data;
                });
            };
            var putLanguageKey = function (languageKey) {
                return $http.post(serviceBase + 'api/Language/PostEditLanguageKey/', languageKey).then(function (x) {
                    return x;
                });
            };
            var getAllLanguages = function () {
                return $http.get(serviceBase + 'api/Language/GetAllLanguages').then(function (x) {
                    return x;
                });
            };

            var putLanguage = function (language) {
                return $http.post(serviceBase + 'api/Language/PostEditLanguage/', language).then(function (x) {
                    return x;
                });
            };

            var putPublishLanguage = function (language) {
                return $http.post(serviceBase + 'api/Language/PostPublishLanguage/', language).then(function (x) {
                    return x;
                });
            };

            var postLanguage = function (language) {
                return $http.post(serviceBase + 'api/Language/PostLanguage/', language).then(function (x) {
                    return x;
                });
            };
            var deleteLanguage = function (language) {
                return $http.post(serviceBase + 'api/Language/PostDeleteLanguage', language).then(function (x) {
                    return x;
                });
            };

            var managementLanguagesServiceFactory = {};
            managementLanguagesServiceFactory.GetLanguageContent = getLanguageContent;
            managementLanguagesServiceFactory.PutLanguageKey = putLanguageKey;
            managementLanguagesServiceFactory.PutLanguage = putLanguage;
            managementLanguagesServiceFactory.PutPublishLanguage = putPublishLanguage;
            managementLanguagesServiceFactory.GetAllLanguages = getAllLanguages;
            managementLanguagesServiceFactory.PostLanguage = postLanguage;
            managementLanguagesServiceFactory.DeleteLanguage = deleteLanguage;
            return managementLanguagesServiceFactory;

        }]);
})();
