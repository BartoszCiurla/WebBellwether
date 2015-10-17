(function () {
    angular
        .module('webBellwether')
        .factory('translateService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;
            var translateServiceFactory = {};

            var getAvailableTranslateServices = function () {
                return $http.get(serviceBase + 'api/Translation/GetAvailableWebServices').then(function (x) {
                    return x;
                });
            };
            function getPrimaryService(services) {
                services.forEach(function (x) {
                    if (x.isPrimary) {
                        return x;
                    }
                });
            };

            var getServiceKey = function (serviceName) {
                return $http.get(serviceBase + 'api/Translation/GetOuterWebServiceKey/?serviceName' + serviceName).then(function (x) {
                    return x.data;
                });
            };

            var translateIntegrationGame = function (currentLangugae, targetLanguage, header, content) {
                var superTest = 'https://translate.yandex.net/api/v1.5/tr.json/translate?key=' + 'trnsl.1.1.20151017T111637Z.54c56d436735854a.e8642bcd77612c2534f47bb494e96fba7fca5c5a' + '&lang='+ currentLangugae + '-' + targetLanguage + '&text=' + header + '&text=' + content;
                return $http.get(superTest).then(function (x) {
                    return x;
                });
                ////first step i need service
                //var availableTranslateService = [];
                //var primaryService = [];
                //var serviceKey = '';

                //return getAvailableTranslateServices().then(function (x) {
                //    //availableTranslateService = x.data;
                //    //second step i need key to service , or not 
                //    //var primaryService = getPrimaryService(availableTranslateService)
                //    primaryservice = x.data;
                //    if (primaryService.useApiKey) {
                //        getServiceKey(primaryService.serviceName).then(function (c) {
                //            serviceKey = c.data;
                //            if (serviceKey == null)
                //                return null;
                //        });
                //    }
                //    //get translation 
                //    return $http.get(primaryService.apiUrl + primaryService.apiKeyTemplate + '=' + serviceKey + '&' + primaryService.languageTemplate + '=' + currentLangugae + '-' + targetLanguage + '&' + primaryService.textInputTemplate + '=' + header + '&' + primaryService.textInputTemplate + '=' + content).then(function (x) {
                //        return x;
                //    });
                //});
            };

            translateServiceFactory.TranslateIntegrationGame = translateIntegrationGame;
            return translateServiceFactory;
        }]);
})();
