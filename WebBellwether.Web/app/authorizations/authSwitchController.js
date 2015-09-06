(function () {
    angular
        .module('webBellwether')
        .controller("authSwitchController", ['$scope', 'sharedService', function ($scope, sharedService) {
            $scope.registration = false;
            $scope.$on('handlePublish', function () {
                $scope.registration = sharedService.sharedmessage;
                console.log("switch get message");
            });


            $scope.joinUs = function () {
                if ($scope.registration) {
                    console.log("switch get message");
                    $scope.registration = false;
                } else {
                    console.log("switch get message");
                    $scope.registration = true;
                }
            };
        }]);
})();