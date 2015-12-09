﻿(function () {
    angular
        .module('webBellwether')
        .controller('integrationGamesController', ['$scope', '$timeout', 'integrationGamesService', 'sharedService', 'startFromFilter', function ($scope, $timeout, integrationGamesService, sharedService, startFromFilter) {
            //pagination
            $scope.search = {};
            $scope.resetFilters = function () {
                $scope.search = {};
            };
            $scope.currentPage = 1;
            $scope.entryLimit = 6; 
            $scope.noOfPages = 0;
            $scope.maxSize = 5; //max size in pager 
            $scope.updateSearch = function (integrationGamesSearchParams) {                
                $scope.search = integrationGamesSearchParams;
                $scope.filtered = startFromFilter($scope.integrationGames, $scope.search);
            };
            $scope.goToTop = function () {
                window.scrollTo(0, 470);
            };
            $scope.pageChanged = function () {
              $scope.goToTop();  
            };
            //**************

            $scope.getIntegrationGames = function (lang) {
                integrationGamesService.IntegrationGames(lang).then(function (x) {
                    $scope.integrationGames = [];
                    $scope.integrationGames = x.data; 
                });
            };
           

            //base init
            function initContent(language) {
                if (language !== undefined) {
                    $scope.getIntegrationGames($scope.selectedLanguage);
                }
            };
            initContent($scope.selectedLanguage);
            // ********************

            $scope.$on('integartiomGamesSearchParamsChange', function () {
                $scope.updateSearch(sharedService.sharedmessage);
            });


            $scope.$on('languageChange', function () {
                $scope.getIntegrationGames(sharedService.sharedmessage);
            });
        }]);
})();
