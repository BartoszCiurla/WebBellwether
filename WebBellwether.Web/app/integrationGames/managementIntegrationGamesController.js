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
                var userNotification = ''
                var integrationGame = {
                    Id: $scope.selectedGame.id,//general id 
                    IntegrationGameId: $scope.selectedGameOtherTranslation.integrationGameId, // id for translation
                    GameName: $scope.selectedGameOtherTranslation.gameName,
                    GameDetails: $scope.selectedGameOtherTranslation.gameDescription,
                    Language: $scope.languageForOtherTranslation.language,
                    Features: []
                }
                $scope.selectedGame.integrationGameDetailModels.forEach(function (x) {
                    integrationGame.Features.push(x.gameFeatureDetailId);
                });
                integrationGamesService.AddNewIntegrationGame(integrationGame).then(function (results) {
                    //success 
                    $scope.selectedGame.gameTranslations.forEach(function (x) {
                        if (x.language.id == integrationGame.Language.id)
                            x.hasTranslation = true;
                    })
                    if (integrationGame.IntegrationGameId == null) {
                        userNotification = $scope.translation.TranslationAdded;
                    }
                    else {
                        userNotification = $scope.translation.TranslationEdited;
                    }
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: userNotification,
                        type: 'success',
                    });

                },
                function (results) {
                    //fails
                    if (integrationGame.IntegrationGameId == null) {
                        userNotification = $scope.translation.TranslationNotAdded;
                    }
                    else
                        userNotification = $scope.translation.TranslationNotEdited;
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                    });
                })

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
            $scope.deleteTranslationDialog = function () {
                var dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.open();
            }
            $scope.deleteGame = function (selectedGame, removeAllTranslation) {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.close();
                dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.close();
                if (removeAllTranslation)
                    selectedGame.temporarySeveralTranslationDelete = removeAllTranslation;
                integrationGamesService.DeleteIntegrationGame(selectedGame).then(function (x) {
                    $scope.getIntegrationGamesWithLanguages($scope.selectedLanguage);
                    console.log(x.data.message)
                    //tutaj jest jednak dupa bo musze mieć osobną funkcję do tego inaczej to nie zadziała ...powód np usuwanie translacji i potem odznaczenie 
                    //jej z listy dostępnych tranlacji ... kurwa 
                    //after delete i must refresh all list 
                },
                function (results) {

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
