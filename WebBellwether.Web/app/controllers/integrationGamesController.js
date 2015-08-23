(function () {
    angular
        .module('webBellwether')
        .controller('integrationGamesController', ['$scope', '$timeout', 'integrationGamesService', function ($scope, $timeout, integrationGamesService) {
            $scope.integrationGamePanel = false;
            $scope.resultStateGameFeature = '';
            $scope.resultStateGameFeatureDetail = '';
            $scope.resultStateNewGame = '';
            $scope.severalLanguagesForGame = false;
            $scope.gameIdForSeveralLanguages = '';
            $scope.changeIntegrationGamePanel = function () {
                if ($scope.integrationGamePanel) {
                    $scope.integrationGamePanel = false;
                } else {
                    $scope.integrationGamePanel = true;
                }
            };

            $scope.setSeveralLanguage = function() {
                if ($scope.severalLanguagesForGame) {
                    $scope.severalLanguagesForGame = false;
                    $scope.gameIdForSeveralLanguages = '';
                }
                else {
                    $scope.severalLanguagesForGame = true;
                }
            };

            var indexedFeatures = [];
            $scope.integrationGames = [];
            $scope.gameFeatureDetails = [];

            function getByProperty(propertyName, propertyValue, collection) {
                var i = 0, len = collection.length;
                for (; i < len; i++) {
                    if (collection[i][propertyName] == +propertyValue) {
                        return collection[i];
                    }
                }
                return null;
            }


            $scope.getIntegrationGames = function (lang) {
                integrationGamesService.IntegrationGames(lang).then(function (results) {
                    $scope.integrationGames = [];
                    $scope.integrationGames = results.data;
                });
            };

            $scope.newGame = function (gameModel) {
                var newIntegrationGameModel =
                {
                    Language: gameModel.language,
                    GameName: gameModel.gameName,
                    GameDetails: gameModel.gameDetails,
                    Features: []
                };
                angular.forEach(gameModel.features, function(x) {
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
                        $scope.getIntegrationGames($scope.selectedLanguage);
                },
                function(results) {
                    
                    //Handling for game several languages
                    if ($scope.severalLanguagesForGame && $scope.gameIdForSeveralLanguages > 0) {
                        $scope.gameIdForSeveralLanguages =parseInt(results.data.message);
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

            $scope.newFeatureDetail = function (gameFeature) {
                var gameFeatureDetail = {
                    Id: gameFeature.featureId,
                    GameFeatureDetailId: '',
                    GameFeature: gameFeature.newFeatureName,
                    Language: gameFeature.language.languageName,
                    LanguageId: gameFeature.language.id
                };
                //BRAK OBSLUGI PUKI CO
                console.log(gameFeatureDetail);
            };

            $scope.getClass = function (id, id2) {
                //for gameFeature
                if (id > 0) {
                    if (id === $scope.resultStateGameFeature)
                        return 'success';
                    if (id + 999 === $scope.resultStateGameFeature)
                        return 'error';
                }
                //for gameFeatureDetail
                if (id2 > 0) {
                    if (id2 === $scope.resultStateGameFeatureDetail)
                        return 'success';
                    if (id2 + 999 === $scope.resultStateGameFeatureDetail)
                        return 'error';
                }
                return '';
            };

            $scope.editGameFeature = function (gameFeature) {
                var gameFeatureModel = {
                    Id: gameFeature.id,
                    GameFeatureName: gameFeature.gameFeatureName,
                    LanguageId:gameFeature.languageId
            };
                integrationGamesService.PutGameFeature(gameFeatureModel).then(function (response) {
                        console.log(response);
                        $scope.resultStateGameFeature = gameFeature.id;
                    },
                function (response) {
                    console.log(response);
                    $scope.resultStateGameFeature = gameFeature.id + 999;
                });
            };

            $scope.editGameFeatureDetail = function (gameFeatureDetail) {
                integrationGamesService.PutGameFeatureDetail(gameFeatureDetail).then(function(response) {
                    console.log(response);
                    $scope.resultStateGameFeatureDetail = gameFeatureDetail.id;
                },
                function(response) {
                    console.log(response);
                    $scope.resultStateGameFeatureDetail = gameFeatureDetail.id + 999;
                });
            };


            $scope.getGameFeatures = function (lang) {
                $scope.getGameFeatureDetails(lang);
                integrationGamesService.GameFeatures(lang).then(function (results) {
                    $scope.gameFeatures = results.data;
                    console.log("get game features");
                });



            };
            //get on load , but i must change this value when i want add game in another language
            $scope.getGameFeatureDetails($scope.selectedLanguage);
            $scope.getIntegrationGames($scope.selectedLanguage);

            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $scope.resultStateNewGame = '';
                }, 4000);
            }
        }]);
})();
