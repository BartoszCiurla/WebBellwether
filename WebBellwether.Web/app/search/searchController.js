(function () {
    angular
        .module('webBellwether')
        .controller('searchController', ['$scope', '$location', 'sharedService', 'jokeService', 'integrationGameService', function ($scope, $location, sharedService, jokeService, integrationGameService) {
            //integration games            
            $scope.integrationGamesSearchParams = {};
            $scope.gameFeatures = [];
            $scope.getGameFeatuesModelWithDetails = function (lang) {
                integrationGameService.GameFeatuesModelWithDetails(lang).then(function (x) {
                    $scope.gameFeatures = x.Data;
                });
            };

            function validateIntegrationGameDetailModelsParam(integrationGamesSearchParams) {
                if (integrationGamesSearchParams.IntegrationGameDetailModels === undefined)
                    return false;
                if (integrationGamesSearchParams.IntegrationGameDetailModels === null)
                    return false;
                return true;
            };

            function validateGameNameParam(integrationGamesSearchParams) {
                if (integrationGamesSearchParams.GameName === undefined)
                    return false;
                if (integrationGamesSearchParams.GameName === null)
                    return false;
                return true;
            }

            function getDetailSearchParam(integrationGamesSearchParams,gameFeatureId) {
                var applyParam = "";
                if (integrationGamesSearchParams.IntegrationGameDetailModels[gameFeatureId] === undefined)
                    return applyParam;
                if (integrationGamesSearchParams.IntegrationGameDetailModels[gameFeatureId] === null)
                    return applyParam;
                return integrationGamesSearchParams.IntegrationGameDetailModels[gameFeatureId].GameFeatureDetailName;
            }

            function validateIntegrationGamesFiltersParams(integrationGamesSearchParams) {
                var searchParams = {}

                var applyGameName = validateGameNameParam(integrationGamesSearchParams);
                if (applyGameName)
                    searchParams.GameName = integrationGamesSearchParams.GameName;

                var applyIntegrationGameDetailsModel = validateIntegrationGameDetailModelsParam(integrationGamesSearchParams);
                if (!applyIntegrationGameDetailsModel)
                    return searchParams;

                var categoryGame = getDetailSearchParam(integrationGamesSearchParams, 0);
                if (categoryGame !== "")
                    searchParams.CategoryGame = categoryGame;
                var paceOfPlay = getDetailSearchParam(integrationGamesSearchParams, 1);
                if (paceOfPlay !== "")
                    searchParams.PaceOfPlay = paceOfPlay;
                var numberOfPlayer = getDetailSearchParam(integrationGamesSearchParams, 2);
                if (numberOfPlayer !== "")
                    searchParams.NumberOfPlayer = numberOfPlayer;
                var preparationFun = getDetailSearchParam(integrationGamesSearchParams, 3);
                if (preparationFun !== "")
                    searchParams.PreparationFun = preparationFun;
                return searchParams;
            };

            $scope.applyIntegrationGamesFilter = function () {
                sharedService.applyIntegrationGamesFilters(validateIntegrationGamesFiltersParams($scope.integrationGamesSearchParams));
            };

            $scope.resetIntegrationGamesFilter = function() {
                $scope.integrationGamesSearchParams = {};
                sharedService.applyIntegrationGamesFilters($scope.integrationGamesSearchParams);
            };
            //******************


            function applyFilterDependingOnLocation(locationPath) {
                if (locationPath === '/game')
                    return "showGamesFilter";
                if (locationPath === '/joke')
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
                jokeService.GetJokeCategories(lang).then(function (x) {
                    $scope.jokeCategories = [];
                    $scope.jokeCategories = x.Data;
                });
            };

            function validateJokeCategoryIdParam(jokeSearchParams) {
                if (jokeSearchParams.JokeCategoryId === undefined)
                    return false;

                if (jokeSearchParams.JokeCategoryId === null)
                    return false;
                return true;
            }
            function validateJokeContentParam(jokeSearchParams) {
                if (jokeSearchParams.JokeContent === undefined)
                    return false;
                if (jokeSearchParams.JokeContent === null)
                    return false;
                return true;
            };
            function validateJokesFiltersParams(jokeSearchParams) {
                var searchParams = {}
                var applyJokeCategoryId = validateJokeCategoryIdParam(jokeSearchParams);
                var applyJokeContent = validateJokeContentParam(jokeSearchParams);
                if (applyJokeCategoryId)
                    searchParams.JokeCategoryId = jokeSearchParams.JokeCategoryId;
                if (applyJokeContent)
                    searchParams.JokeContent = jokeSearchParams.JokeContent;
                return searchParams;
            };

            $scope.applyJokesFilter = function () {
                sharedService.applyJokesFilter(validateJokesFiltersParams($scope.jokesSearchParams));
            };
            $scope.resetJokesFilter = function() {
                $scope.jokesSearchParams = {};
                sharedService.applyJokesFilter($scope.jokesSearchParams);
            };
            //******************

            $scope.changeLocation = function(url) {
                $location.path(url);
            };

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
