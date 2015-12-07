(function () {
    angular
        .module('webBellwether')
        .controller('searchController', ['$scope', 'sharedService', 'jokesService', 'integrationGamesService', function ($scope, sharedService, jokesService, integrationGamesService) {
            //integration games
            $scope.integrationGamesSearchParams = {};
            $scope.gameFeatures = [];
            $scope.getGameFeatuesModelWithDetails = function (lang) {
                integrationGamesService.GameFeatuesModelWithDetails(lang).then(function (x) {
                    $scope.gameFeatures = x.data;
                });
            };
            $scope.applyIntegrationGamesFilter = function() {
                sharedService.applyIntegrationGamesFilters($scope.integrationGamesSearchParams);
            };
            //******************


            //jokes
            //******************

            

            //base init
            function initContent(language) {
                if(language !== undefined){
                    $scope.getGameFeatuesModelWithDetails(language);
                }
            };
            initContent($scope.selectedLanguage);
            //******************

            //when language change
            $scope.$on('languageChange', function () {
                initContent(sharedService.sharedmessage);
            });
            //******************
        }]);
})();
