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
            var managementLanguagesServiceFactory = {};
            managementLanguagesServiceFactory.GetLanguageContent = getLanguageContent;
            return managementLanguagesServiceFactory;

        }]);
})();
