(function () {
    angular
        .module('webBellwether')
        .controller('searchController', ['$scope', '$location', 'sharedService', 'jokesService', 'integrationGamesService', function ($scope, $location, sharedService, jokesService, integrationGamesService) {
            //integration games            
            $scope.integrationGamesSearchParams = {};
            $scope.gameFeatures = [];
            $scope.getGameFeatuesModelWithDetails = function (lang) {
                integrationGamesService.GameFeatuesModelWithDetails(lang).then(function (x) {
                    $scope.gameFeatures = x.data;
                });
            };
            $scope.applyIntegrationGamesFilter = function () {
                sharedService.applyIntegrationGamesFilters($scope.integrationGamesSearchParams);
            };
            //******************


            function applyFilterDependingOnLocation(locationPath) {
                if (locationPath === '/games')
                    return "showGamesFilter";
                if (locationPath === '/jokes')
                    return "showJokesFilter";
                return '';
            };
            $scope.$watch(function () {
                return $location.path();
            }, function (locationPath) {
                $scope.filterDependingOnLocation = applyFilterDependingOnLocation(locationPath);
            });
            $scope.filterDependingOnLocation = '';


            //jokes            
            $scope.jokesSearchParams = {};
            $scope.jokeCategories = [];
            $scope.getJokeCategories = function (lang) {
                jokesService.GetJokeCategories(lang).then(function (x) {
                    $scope.jokeCategories = [];
                    $scope.jokeCategories = x.data;
                });
            };

            function validateJokeCategoryIdParam(jokeSearchParams) {
                if (jokeSearchParams.jokeCategoryId === undefined)
                    return false;

                if (jokeSearchParams.jokeCategoryId === null)
                    return false;
                return true;
            }
            function validateJokeContentParam(jokeSearchParams) {
                if (jokeSearchParams.jokeContent === undefined)
                    return false;
                if (jokeSearchParams.jokeContent === null)
                    return false;
                return true;
            };
            function validateJokesFiltersParams(jokeSearchParams) {
                var searchParams = {}
                var applyJokeCategoryId = validateJokeCategoryIdParam(jokeSearchParams);
                var applyJokeContent = validateJokeContentParam(jokeSearchParams);
                if (applyJokeCategoryId)
                    searchParams.jokeCategoryId = jokeSearchParams.jokeCategoryId;
                if (applyJokeContent)
                    searchParams.jokeContent = jokeSearchParams.jokeContent;
                return searchParams;
            };

            $scope.applyJokesFilter = function () {
                sharedService.applyJokesFilter(validateJokesFiltersParams($scope.jokesSearchParams));
            };
            //******************



            //base init
            function initContent(language) {
                if (language !== undefined) {
                    $scope.getGameFeatuesModelWithDetails(language);
                    $scope.getJokeCategories(language);
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
