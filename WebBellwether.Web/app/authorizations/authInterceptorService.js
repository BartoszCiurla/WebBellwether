(function () {
    angular
        .module('webBellwether')
        .factory('authInterceptorService', ['$q', '$injector', '$location', 'localStorageService', function ($q, $injector, $location, localStorageService) {

            var authInterceptorServiceFactory = {};

            var _request = function (config) {

                config.headers = config.headers || {};

                var authData = localStorageService.get('authorizationData');
                if (authData) {
                    //This option is for testing time
                    var test = config.url.substr(0,17)
                    if (angular.equals(test, 'https://translate'))
                        console.log("Other service");
                        //This option is for testing time
                    else {
                        config.headers.Authorization = 'Bearer ' + authData.token;
                    }
                }
                return config;
            }

            var _responseError = function (rejection) {
                if (rejection.status === 401) {
                    var authService = $injector.get('authService');
                    var authData = localStorageService.get('authorizationData');

                    if (authData) {
                        if (authData.useRefreshTokens) {
                            $location.path('/refresh');
                            return $q.reject(rejection);
                        }
                    }
                    authService.logOut();
                    $location.path('/login');
                }
                return $q.reject(rejection);
            }

            authInterceptorServiceFactory.request = _request;
            authInterceptorServiceFactory.responseError = _responseError;

            return authInterceptorServiceFactory;
        }]);
})();
