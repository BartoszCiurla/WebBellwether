(function () {
    angular
    .module('webBellwether')
    .factory('sharedService', ['$rootScope', function ($rootScope) {
        var sharedService = {};
        sharedService.sharedmessage = '';

        sharedService.languageChange = function (language) {
            this.sharedmessage = language;
            $rootScope.$broadcast('languageChange');
        }

        sharedService.applyIntegrationGamesFilters = function (integartiomGamesSearchParams) {
            this.sharedmessage = integartiomGamesSearchParams;
            $rootScope.$broadcast("integartiomGamesSearchParamsChange");
        };
        sharedService.applyJokesFilter = function (jokesSearchParams) {
            this.sharedmessage = jokesSearchParams;
            $rootScope.$broadcast("jokesSearchParamsChange");
        };

        sharedService.prepForPublish = function (msg) {
            this.sharedmessage = msg;
            this.publishItem();
        };
        sharedService.publishItem = function () {
            $rootScope.$broadcast('handlePublish');
        };
        return sharedService;
    }]);
})();

