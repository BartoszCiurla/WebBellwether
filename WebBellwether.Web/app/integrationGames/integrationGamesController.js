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
            $scope.entryLimit = 6; 
            $scope.noOfPages = 0;
            $scope.maxSize = 5; //max size in pager 
            $scope.updateSearch = function (integrationGamesSearchParams) {
                //var modelForGameFilters = {
                //    gameName: integrationGamesSearchParams.gameName,
                //    integrationGameDetailModels: []
                //};
                //tutaj musi iść konkretne mapowanko struktura niby taka sama ale kórrwa równie bywa ...                 
                //chwilowo zaprzestaje rozwoju tego miejsca gdyż przerasta mnie skomplikowanie kodu zrobie moduł z jokami na gotowo i w nim w pierwszej kolejności zastosuje filtry
                $scope.search = integrationGamesSearchParams;
                $scope.filtered = startFromFilter($scope.integrationGames, integrationGamesSearchParams );
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
