(function () {
    angular
        .module('webBellwether')
        .controller('managementIntegrationGamesController', ['$scope', '$timeout', 'integrationGamesService','sharedService', function ($scope, $timeout, integrationGamesService,sharedService) {
            //pagination
            $scope.currentPage = 0;
            $scope.pageSize = 14;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.integrationGames.length / $scope.pageSize);
            }
            // ********************

            //integration games
            $scope.integrationGames = [];
            $scope.getIntegrationGamesWithLanguages = function (lang) {
                integrationGamesService.IntegrationGamesWithAvailableLanguages(lang).then(function (x) {
                    $scope.integrationGames = [];
                    $scope.integrationGames = x.data;
                });
            };
            // ********************

            //game features 
            $scope.gameFeatures = [];
            $scope.getGameFeatuesModelWithDetails = function(lang) {
                integrationGamesService.GameFeatuesModelWithDetails(lang).then(function (x) {
                    $scope.gameFeatures = x.data;
                });
            }            
            // ********************

            //base init
            $scope.getIntegrationGamesWithLanguages($scope.selectedLanguage);
            $scope.getGameFeatuesModelWithDetails($scope.selectedLanguage);
            // ********************

            //when language change 
            $scope.$on('languageChange', function () {
                $scope.getIntegrationGamesWithLanguages(sharedService.sharedmessage);
                $scope.getGameFeatuesModelWithDetails(sharedService.sharedmessage);
            });
            // ********************
        }]);
})();
