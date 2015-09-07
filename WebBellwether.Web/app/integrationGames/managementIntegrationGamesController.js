(function () {
    angular
        .module('webBellwether')
        .controller('managementIntegrationGamesController', ['$scope', '$timeout', 'integrationGamesService', function ($scope, $timeout, integrationGamesService) {
            $scope.currentPage = 0;
            $scope.pageSize = 14;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.integrationGames.length / $scope.pageSize);
            }
            $scope.integrationGames = [];
            $scope.getIntegrationGamesWithLanguages = function (lang) {
                integrationGamesService.IntegrationGamesWithAvailableLanguages(lang).then(function (results) {
                    $scope.integrationGames = [];
                    $scope.integrationGames = results.data;
                });
            };
            //potrzebny mi system który na podstawie zwrotki z bazy zbuduje kilka select boxow kazdy z nich bedzie mogl przyjac tylko te odpowiednie detale
            $scope.GameFeatures = [];


            $scope.getIntegrationGamesWithLanguages($scope.selectedLanguage);
        }]);
})();
