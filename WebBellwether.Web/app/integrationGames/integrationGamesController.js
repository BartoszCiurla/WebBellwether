(function () {
    angular
        .module('webBellwether')
        .controller('integrationGamesController', ['$scope', '$timeout', 'integrationGamesService', 'sharedService', 'startFromFilter', function ($scope, $timeout, integrationGamesService, sharedService, startFromFilter) {
            //pagination
            $scope.search = {};
            $scope.resetFilters = function () {
                $scope.search = {};
            };
            $scope.currentPage = 1;
            $scope.entryLimit = 6; // items per page
            $scope.noOfPages = 0;
            $scope.maxSize = 5; //max size in pager 
            $scope.updateSearch = function() {
                $scope.filtered = startFromFilter($scope.integrationGames,  $scope.search.gameName );
            };
            $scope.goToTop = function () {
                window.scrollTo(0, 470);
            };
            $scope.pageChanged = function () {
              $scope.goToTop();  
            };
            //**************

            $scope.getIntegrationGames = function (lang) {
                integrationGamesService.IntegrationGames(lang).then(function (results) {
                    $scope.integrationGames = [];
                    $scope.integrationGames = results.data; 
                });
            };
            $scope.getIntegrationGames($scope.selectedLanguage);

            //when language change 
            $scope.$on('languageChange', function () {
                $scope.getIntegrationGames(sharedService.sharedmessage);
            });
        }]);
})();
