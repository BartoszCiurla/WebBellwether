(function () {
    angular
        .module('webBellwether')
        .controller('integrationGamesController', ['$scope','$timeout','integrationGamesService','sharedService', function ($scope, $timeout, integrationGamesService, sharedService) {
            $scope.currentPage = 0;
            $scope.pageSize = 12;
            $scope.integrationGames = [];
            $scope.goToTop = function () {
                window.scrollTo(0, 470);
            };
            $scope.numberOfPages = function () {
                return Math.ceil($scope.integrationGames.length / $scope.pageSize);
            }
            $scope.getIntegrationGames = function (lang) {
                integrationGamesService.IntegrationGames(lang).then(function (results) {
                    $scope.integrationGames = [];
                    $scope.integrationGames = results.data;
                });
            };
            $scope.getIntegrationGames($scope.selectedLanguage);


         


            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $scope.resultStateNewGame = '';
                }, 4000);
            }

            //when language change 
            $scope.$on('languageChange', function () {
                $scope.getIntegrationGames(sharedService.sharedmessage);
            });
        }]);
})();
