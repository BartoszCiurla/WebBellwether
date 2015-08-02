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

            $scope.integrationGames = [];
            $scope.gameCategories = '';
            $scope.numberOfPlayers = '';
            $scope.paceOfPlays = '';
            $scope.preparationFuns = '';
            $scope.gameFeatures = {};
            $scope.selectedGameFeatureId = '';
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
                var integrationGameModel = {
                    GameName: gameModel.gameName,
                    GameDetails: gameModel.gameDetails,
                    GameCategoryId: gameModel.gameCategory,
                    GameCategory: getByProperty('id', gameModel.gameCategory, $scope.gameCategories).gameCategoryName,
                    PaceOfPlayId: gameModel.paceOfPlay,
                    PaceOfPlay: getByProperty('id', gameModel.paceOfPlay, $scope.paceOfPlays).paceOfPlayName,
                    NumberOfPlayerId: gameModel.numberOfPlayer,
                    NumberOfPlayer: getByProperty('id', gameModel.numberOfPlayer, $scope.numberOfPlayers).numberOfPlayerName,
                    PreparationFunId: gameModel.preparationFun,
                    PreparationFun: getByProperty('id', gameModel.preparationFun, $scope.preparationFuns).preparationFunName,
                    LanguageId: gameModel.language.id,
                    Language: gameModel.language.languageName
                }
                integrationGamesService.AddNewIntegrationGame(integrationGameModel).then(function (results) {
                    gameModel = '';
                    console.log("post new integration game");
                });
            };

            $scope.getGameDescription = function (lang) {
                integrationGamesService.GameDescription(lang).then(function (results) {
                    console.log("blad? to jakies zarty kur..");
                    $scope.gameCategories = results.data['gameCategories'];
                    $scope.numberOfPlayers = results.data['numberOfPlayers'];
                    $scope.paceOfPlays = results.data["paceOfPlays"];
                    $scope.preparationFuns = results.data["preparationFuns"];
                    console.log("get game details model");
                    $scope.fillGameFeatureDetails($scope.selectedGameFeatureId);
                    console.log("fill game feature details");
                });
            };

            $scope.fillGameFeatureDetails = function (featureId) {
                $scope.gameFeatureDetails = [];
                $scope.selectedGameFeatureId = featureId;

                switch (parseInt(featureId)) {
                    case 1:
                        angular.forEach($scope.gameCategories, function (item) {
                            $scope.gameFeatureDetails.push({ key: item.id, value: item.gameCategoryName });
                        });
                        break;
                    case 2:
                        $scope.gameFeatureDetails = [];
                        angular.forEach($scope.paceOfPlays, function (item) {
                            $scope.gameFeatureDetails.push({ key: item.id, value: item.paceOfPlayName });
                        });
                        break;
                    case 3:
                        $scope.gameFeatureDetails = [];
                        angular.forEach($scope.numberOfPlayers, function (item) {
                            $scope.gameFeatureDetails.push({ key: item.id, value: item.numberOfPlayerName });
                        });
                        break;
                    case 4:
                        $scope.gameFeatureDetails = [];
                        angular.forEach($scope.preparationFuns, function (item) {
                            $scope.gameFeatureDetails.push({ key: item.id, value: item.preparationFunName });
                        });
                        break;
                    default:
                }
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

            $scope.editGameFeatureDetail = function (gameFeature, gfd) {
                var gameFeatureDetail = {
                    Id: gameFeature.featureId,
                    GameFeatureDetailId: gfd.key,
                    GameFeature: gfd.value,
                    Language: gameFeature.language.languageName,
                    LanguageId: gameFeature.language.id
                };
                console.log(gameFeatureDetail);
            };


            $scope.getGameFeatures = function (lang) {
                $scope.getGameDescription(lang);
                integrationGamesService.GameFeatures(lang).then(function (results) {
                    $scope.gameFeatures = results.data;
                    console.log("get game features");
                });



            };
            //get on load , but i must change this value when i want add game in another language
            $scope.getGameDescription($scope.selectedLanguage);
            $scope.getIntegrationGames($scope.selectedLanguage);


        }]);
})();
