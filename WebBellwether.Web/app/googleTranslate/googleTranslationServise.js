(function () {
    angular
        .module('webBellwether')
        .factory('googleTranslationService', ['$http', 'ngAuthSettings', function ($htttp, ngAuthSettings) {
            var googleTranslationService = {};

            var getIntegrationGameTranslation = function () {

            };

            googleTranslationService.GetIntegrationGameTranslation = getIntegrationGameTranslation;
            return googleTranslationService;
        }])
})();