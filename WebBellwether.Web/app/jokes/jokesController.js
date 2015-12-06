(function () {
    angular
        .module('webBellwether')
        .controller('jokesController', ['$scope', function ($scope) {
            $scope.location = $location;
        }]);
})();