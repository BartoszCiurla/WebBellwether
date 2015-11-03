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


            var managementLanguagesServiceFactory = {};
            managementLanguagesServiceFactory.GetLanguageContent = getLanguageContent;
            managementLanguagesServiceFactory.PutLanguageKey = putLanguageKey;
            return managementLanguagesServiceFactory;

        }]);
})();
