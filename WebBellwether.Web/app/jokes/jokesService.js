(function () {
    angular
        .module('webBellwether')
        .factory('jokesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var jokesServiceFactory = {};
            return jokesServiceFactory;

        }]);
})();
