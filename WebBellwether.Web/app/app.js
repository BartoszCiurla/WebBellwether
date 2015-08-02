(function () {
    angular.module('webBellwether', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'ngResource']);
})();


(function () {
    angular
        .module('webBellwether')
        .constant('ngAuthSettings', {
            //*************************
            apiServiceBaseUri: '',
            //*************************
            clientId: 'ngAuthApp'

        });
})();


(function () {
    angular
        .module('webBellwether')
        .config(function ($httpProvider) {
            $httpProvider.interceptors.push('authInterceptorService');
        });
})();

(function () {
    angular
        .module('webBellwether')
        .run(['authService', function (authService) {
            authService.fillAuthData();
        }]);
})();


