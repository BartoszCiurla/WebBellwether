(function () {
    angular
        .module('webBellwether')
        .controller('newGameManagementController', ['$scope', '$timeout', 'integrationGamesService', function ($scope, $timeout, integrationGamesService) {
            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $scope.resultStateNewGame = '';
                }, 4000);
            };
            $scope.resultStateGameFeature = '';
            $scope.resultStateGameFeatureDetail = '';
            $scope.resultStateNewGame = '';
            $scope.severalLanguagesForGame = false;
            $scope.gameIdForSeveralLanguages = '';
            $scope.setSeveralLanguage = function () {
                if ($scope.severalLanguagesForGame) {
                    $scope.severalLanguagesForGame = false;
                    $scope.gameIdForSeveralLanguages = '';
                }
                else {
                    $scope.severalLanguagesForGame = true;
                }
            };
            var indexedFeatures = [];
            $scope.gameFeatureDetails = [];

            $scope.newGame = function (gameModel) {
                var newIntegrationGameModel =
                {
                    Language: gameModel.language,
                    GameName: gameModel.gameName,
                    GameDetails: gameModel.gameDetails,
                    Features: []
                };
                angular.forEach(gameModel.features, function (x) {
                    newIntegrationGameModel.Features.push(x);
                });
                console.log(newIntegrationGameModel);

                //Handling for game several languages
                if ($scope.severalLanguagesForGame && $scope.gameIdForSeveralLanguages > 0) {
                    newIntegrationGameModel.ID = $scope.gameIdForSeveralLanguages;
                }

                integrationGamesService.AddNewIntegrationGame(newIntegrationGameModel).then(function (results) {
                    console.log(results);

                    //Handling for game several languages
                    if ($scope.severalLanguagesForGame) {
                        $scope.gameIdForSeveralLanguages = results.data;
                        $scope.resultStateNewGame = 3;//several language game success
                    } else {
                        $scope.resultStateNewGame = 1;//single language game success
                    }
                    startTimer();
                },
                function (results) {

                    //Handling for game several languages
                    if ($scope.severalLanguagesForGame && $scope.gameIdForSeveralLanguages > 0) {
                        $scope.gameIdForSeveralLanguages = parseInt(results.data.message);
                        $scope.resultStateNewGame = 4; //several language game error
                    } else {
                        $scope.resultStateNewGame = 2;//single language game error
                    }
                    startTimer();
                });
            };

            $scope.featuresToFilter = function () {
                indexedFeatures = [];
                return $scope.gameFeatureDetails;
            }

            $scope.filterFeatures = function (feature) {
                var featureIsNew = indexedFeatures.indexOf(feature.gameFeatureId) == -1;
                if (featureIsNew) {
                    indexedFeatures.push(feature.gameFeatureId);
                }
                return featureIsNew;
            }

            $scope.getGameFeatureDetails = function (lang) {
                integrationGamesService.GameFeatureDetails(lang).then(function (results) {
                    $scope.gameFeatureDetails = results.data;
                    console.log("fill game feature details");
                });
            };

            $scope.getGameFeatureDetails($scope.selectedLanguage);
        }]);
})();
