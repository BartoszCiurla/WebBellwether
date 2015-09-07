﻿(function () {
    angular
        .module('webBellwether')
        .controller('integrationGamesController', ['$scope', '$timeout', 'integrationGamesService', function ($scope, $timeout, integrationGamesService) {
            $scope.currentPage = 0;
            $scope.pageSize = 12;
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
        }]);
})();