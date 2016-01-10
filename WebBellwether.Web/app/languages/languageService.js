(function () {
    angular
        .module('webBellwether')
        .factory('languageService', ['$http', '$resource', 'ngAuthSettings', function ($http, $resource, ngAuthSettings) {
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
                    return x.data;
                });
            };
            var getAllLanguages = function () {
                return $http.get(serviceBase + 'api/Language/GetAllLanguages').then(function (x) {
                    return x.data;
                });
            };

            var putLanguage = function (language) {
                return $http.post(serviceBase + 'api/Language/PostEditLanguage/', language).then(function (x) {
                    return x.data;
                });
            };

            var putPublishLanguage = function (language) {
                return $http.post(serviceBase + 'api/Language/PostPublishLanguage/', language).then(function (x) {
                    return x.data;
                });
            };

            var postLanguage = function (language) {
                return $http.post(serviceBase + 'api/Language/PostLanguage/', language).then(function (x) {
                    return x.data;
                });
            };
            var deleteLanguage = function (language) {
                return $http.post(serviceBase + 'api/Language/PostDeleteLanguage', language).then(function (x) {
                    return x.data;
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
