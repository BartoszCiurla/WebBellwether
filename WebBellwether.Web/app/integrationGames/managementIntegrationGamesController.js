(function () {
    angular
        .module('webBellwether')
        .controller('managementIntegrationGamesController', ['$scope', '$timeout', 'integrationGamesService', 'sharedService', function ($scope, $timeout, integrationGamesService, sharedService) {
            //pagination and set other game translation (get from api)
            $scope.currentPage = 0;
            $scope.pageSize = 10;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.integrationGames.length / $scope.pageSize);
            }
            $scope.selectedGame = '';
            $scope.selectedGameOtherTranslation = '';
            $scope.languageForOtherTranslation = '';
            $scope.setSelectGame = function (selected) {
                if (selected.id == $scope.selectedGame.id) {
                    $scope.selectedGame = '';
                    $scope.selectedGameOtherTranslation = '';
                } else {
                    $scope.selectedGame = selected;
                    $scope.getTranslationForGame();
                                  
                }
            };
            $scope.getTranslationForGame = function () {
                //check language on new translation 
                if ($scope.languageForOtherTranslation.language != null) {
                    //check available translation by language in selectedGame
                    $scope.selectedGame.gameTranslations.forEach(function (o) {
                        if (o.language.id == $scope.languageForOtherTranslation.language.id && o.hasTranslation) {
                            $scope.selectedGameOtherTranslation.id = $scope.selectedGame.id;      
                            integrationGamesService.GetIntegrationGameTranslation($scope.selectedGame.id, $scope.languageForOtherTranslation.language.id).then(function (x) {
                                $scope.selectedGameOtherTranslation = x.data;
                            });
                            return;
                        }
                    });
                }
                $scope.selectedGameOtherTranslation = '';
            }
            // ********************


            //edit insert new translation
            $scope.saveOtherTranslation = function () {
                var integrationGame = {
                    Id: $scope.selectedGame.id,
                    IntegrationGameId :$scope.selectedGameOtherTranslation.integrationGameId,
                    GameName : $scope.selectedGameOtherTranslation.gameName,
                    GameDescription :$scope.selectedGameOtherTranslation.gameDescription,
                    Language: $scope.languageForOtherTranslation.language,
                    IntegrationGameDetailModels: $scope.selectedGame.integrationGameDetailModels
                }
                integrationGamesService.SaveOtherGameTranslation(integrationGame);
            };


            //integration games
            $scope.integrationGames = [];
            $scope.getIntegrationGamesWithLanguages = function (lang) {
                integrationGamesService.IntegrationGamesWithAvailableLanguages(lang).then(function (x) {
                    $scope.integrationGames = [];
                    $scope.integrationGames = x.data;
                    $scope.setGameTranslationAfterLanguageChange();                    
                });
            };

            $scope.setGameTranslationAfterLanguageChange = function () {
                if ($scope.selectedGame != '') {
                    $scope.integrationGames.forEach(function (o) {
                        if (o.id == $scope.selectedGame.id && o.language.id == $scope.selectedLanguage) {
                            $scope.selectedGame = o;
                            return;
                        }
                    });
                    if ($scope.selectedGame.language.id != $scope.selectedLanguage)
                        $scope.selectedGame = '';
                }
            }
            // ********************

            //edit integration games
            $scope.editGame = function (selectedGame) {
                integrationGamesService.PutIntegrationGame(selectedGame).then(function (x) {
                    //tutaj trzeba by jeszcze obsluzyc jakies informowanie usera 
                });
            };
            // ********************

            //delete integration games
            $scope.deleteDialog = function () {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.open();
            }
            $scope.countGameTranslation = function (selectedGame) {
                var result = 0;
                selectedGame.gameTranslations.forEach(function (x) {
                    if (x.hasTranslation)
                        result++;
                })
                return result;
            }
            $scope.deleteGame = function (selectedGame, removeAllTranslation) {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.close();
                if (removeAllTranslation)
                    selectedGame.temporarySeveralTranslationDelete = removeAllTranslation;
                integrationGamesService.DeleteIntegrationGame(selectedGame).then(function (x) {
                    $scope.getIntegrationGamesWithLanguages($scope.selectedLanguage);
                    //after delete i must refresh all list 
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
            $scope.getIntegrationGamesWithLanguages($scope.selectedLanguage);
            $scope.getGameFeatuesModelWithDetails($scope.selectedLanguage);
            // ********************

            //when language change 
            $scope.$on('languageChange', function () {
                $scope.getIntegrationGamesWithLanguages(sharedService.sharedmessage);
                $scope.getGameFeatuesModelWithDetails(sharedService.sharedmessage);              
            });
            // ********************


        }]);
})();
