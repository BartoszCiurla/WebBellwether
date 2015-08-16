(function () {
    angular
        .module('webBellwether')
        .controller('integrationGamesController', ['$scope', 'integrationGamesService', function ($scope, integrationGamesService) {
            $scope.integrationGamePanel = false;
            $scope.changeIntegrationGamePanel = function () {
                if ($scope.integrationGamePanel) {
                    $scope.integrationGamePanel = false;
                } else {
                    $scope.integrationGamePanel = true;
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
                    $scope.integrationGames = results.data;
                });
            };

            $scope.newGame = function (gameModel) {
               // console.log(gameModel);
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
                integrationGamesService.AddNewIntegrationGame(newIntegrationGameModel).then(function (results) {
                    gameModel = '';
                    //console.log("post new integration game");
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
            //dziwna opcja 


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
                console.log(gameFeatureDetail);
            };

            $scope.editGameFeature = function (gameFeature) {
                var gameFeatureModel = {
                    Id: gameFeature.id,
                    GameFeatureName: gameFeature.gameFeatureName,
                    LanguageId:gameFeature.languageId,
            };
                integrationGamesService.PutGameFeature(gameFeatureModel);
                //tutaj jeszcze można się pokosić o jakaś wiadomość dla usera 
            };

            $scope.editGameFeatureDetail = function (gameFeatureDetail) {
                integrationGamesService.PutGameFeatureDetail(gameFeatureDetail);
                console.log(gameFeatureDetail);
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


        }]);
})();
