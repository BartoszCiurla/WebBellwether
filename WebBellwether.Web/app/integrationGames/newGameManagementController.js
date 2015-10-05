(function () {
    angular
        .module('webBellwether')
        .controller('newGameManagementController', ['$scope', '$timeout', 'integrationGamesService', function ($scope, $timeout, integrationGamesService) {
            //timer for user notification
            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    if ($scope.resultStateNewGame == 1 | $scope.resultStateNewGame == 3) {
                        var dialog = $('#successDialog').data('dialog');
                        dialog.close();
                    }
                    else {
                        var dialog = $('#failDialog').data('dialog');
                        dialog.close();
                    }
                    $scope.resultStateNewGame = '';
                    
                }, 4000);
            };
            
            //insert game and notify user 
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
                    //get dialog and open it 
                    var dialog = $('#successDialog').data('dialog');
                    dialog.open();   
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
                    var dialog = $('#failDialog').data('dialog');
                    dialog.open();
                    startTimer();
                });
            };
            // ********************

            //game features 
            $scope.gameFeatures = [];
            $scope.getGameFeatuesModelWithDetails = function (lang) {
                integrationGamesService.GameFeatuesModelWithDetails(lang).then(function (x) {
                    $scope.gameFeatures = x.data;
                });
            };
            // ********************

            //base init
            $scope.getGameFeatuesModelWithDetails($scope.selectedLanguage);
            // ********************
        }]);
})();
