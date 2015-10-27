(function () {
    angular
        .module('webBellwether')
        .factory('languagesService', ['$http','$resource', 'ngAuthSettings', function ($http,$resource, ngAuthSettings) {
            var serviceBase = ngAuthSettings.apiServiceBaseUri;
            var getContent = function (lang) {
                var languageFilePath = '/appData/translations/translation_' + lang + '.json';
                return $resource(languageFilePath).get(function (data) {
                    return data;
                });
            };
            var managementLanguagesServiceFactory = {};
            managementLanguagesServiceFactory.GetContent = getContent;
            return managementLanguagesServiceFactory;

        }]);
})();
