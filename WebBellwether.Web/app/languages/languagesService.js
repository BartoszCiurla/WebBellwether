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
            //var result = [];
            //for (key in data) {
            //    result.push({ Key: key, Value: data[key] });
            //}
            //return result;

            //var resultData = [];
            //var result = $resource(languageFilePath);
            //result.get()
            //.$promise.then(function (x) {
            //    for (key in x) {
            //        resultData.push({ Key: key, Value: data[key] });
            //    }
            //    return resultData;
            //})

            var managementLanguagesServiceFactory = {};
            managementLanguagesServiceFactory.GetLanguageContent = getLanguageContent;
            return managementLanguagesServiceFactory;

        }]);
})();
