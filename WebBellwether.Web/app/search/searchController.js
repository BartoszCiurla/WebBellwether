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

            function validateIntegrationGameDetailModelsParam(integrationGamesSearchParams) {
                if (integrationGamesSearchParams.integrationGameDetailModels === undefined)
                    return false;
                if (integrationGamesSearchParams.integrationGameDetailModels === null)
                    return false;
                return true;
            };

            function validateGameNameParam(integrationGamesSearchParams) {
                if (integrationGamesSearchParams.gameName === undefined)
                    return false;
                if (integrationGamesSearchParams.gameName === null)
                    return false;
                return true;
            }

            function getDetailSearchParam(integrationGamesSearchParams,gameFeatureId) {
                var applyParam = "";
                if (integrationGamesSearchParams.integrationGameDetailModels[gameFeatureId] === undefined)
                    return applyParam;
                if (integrationGamesSearchParams.integrationGameDetailModels[gameFeatureId] === null)
                    return applyParam;
                return integrationGamesSearchParams.integrationGameDetailModels[gameFeatureId].gameFeatureDetailName;
            }

            function validateIntegrationGamesFiltersParams(integrationGamesSearchParams) {
                var searchParams = {}

                var applyGameName = validateGameNameParam(integrationGamesSearchParams);
                if (applyGameName)
                    searchParams.gameName = integrationGamesSearchParams.gameName;

                var applyIntegrationGameDetailsModel = validateIntegrationGameDetailModelsParam(integrationGamesSearchParams);
                if (!applyIntegrationGameDetailsModel)
                    return searchParams;

                //              TAK WIEM MAM PEŁNĄ ŚWIADOMOŚĆ JAK BARDZO PONIŻSZY KOD JEST SPIERDOLONY 
                //  ZOSTAWIAM GO W TAKIEJ FORMIE BO BRAKUJE MI CZASU ALE Z PEWNOŚCIĄ BEDZIE TO NAUCZKA NA PRZYSZŁOŚĆ 
                //              WYDAJE MI SIĘ ŻE ANALIZUJĄC GO DOKŁADNIE MOŻNA ODNALEŚĆ PEWNE WADY ANGULARA ... 
                //                                  LUB MÓJ POMYSŁ BYŁ ZJEBANY ...
                //   Z DRUGIEJ STRONY MOGLEM ULEPSZYĆ FILTR ALE TO TRWAŁO BY TYLE SAMO I PEWNIE MOGLO BY NIE DZIALAĆ

                var categoryGame = getDetailSearchParam(integrationGamesSearchParams, 0);
                if (categoryGame !== "")
                    searchParams.categoryGame = categoryGame;
                var paceOfPlay = getDetailSearchParam(integrationGamesSearchParams, 1);
                if (paceOfPlay !== "")
                    searchParams.paceOfPlay = paceOfPlay;
                var numberOfPlayer = getDetailSearchParam(integrationGamesSearchParams, 2);
                if (numberOfPlayer !== "")
                    searchParams.numberOfPlayer = numberOfPlayer;
                var preparationFun = getDetailSearchParam(integrationGamesSearchParams, 3);
                if (preparationFun !== "")
                    searchParams.preparationFun = preparationFun;
                return searchParams;
            };

            $scope.applyIntegrationGamesFilter = function () {
                sharedService.applyIntegrationGamesFilters(validateIntegrationGamesFiltersParams($scope.integrationGamesSearchParams));
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
