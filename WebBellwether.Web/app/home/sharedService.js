(function () {
    angular
    .module('webBellwether')
    .factory('sharedService',['$rootScope', function ($rootScope) {
        var sharedService = {};

        sharedService.sharedmessage = '';

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

