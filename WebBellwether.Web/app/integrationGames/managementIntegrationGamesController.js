﻿(function () {
    angular
        .module('webBellwether')
        .controller('managementIntegrationGamesController', ['$scope', '$timeout', 'integrationGamesService', 'translateService', 'sharedService', function ($scope, $timeout, integrationGamesService, translateService, sharedService) {
            var userNotification = '';
            //pagination and set other game translation (get from api)
            $scope.currentPage = 0;
            $scope.pageSize = 10;
            $scope.numberOfPages = function () {
                if ($scope.integrationGames.length > 0)
                    return Math.ceil($scope.integrationGames.length / $scope.pageSize);
                else
                    return 0;
            }
            $scope.selectedGame = '';
            $scope.selectedGameOtherTranslation = '';
            $scope.languageForOtherTranslation = '';
            $scope.setSelectGame = function (selected) {
                if (selected.id == $scope.selectedGame.id) {
                    $scope.resetSelectedGameOrTranslation(true, true);
                } else {
                    $scope.selectedGame = selected;
                    $scope.getTranslationForGame();
                }
            };
            //test
            $scope.translateGame = function (currentLanguage, targetLanguage, gameName, gameDescription) {
                //fast code for test 
                var tempCurrentLang = '';
                var tempTargetLang = '';
                if (currentLanguage == 1)
                    tempCurrentLang = 'en';
                else
                    tempCurrentLang = 'pl';
                if (targetLanguage = 1)
                    tempTargetLang = 'en';
                else
                    tempTargetLang = 'pl';
                translateService.TranslateIntegrationGame(tempCurrentLang, tempTargetLang, gameName, gameDescription).then(function (x) {
                    $scope.selectedGameOtherTranslation = {};
                    $scope.selectedGameOtherTranslation.gameName = x.data.text[0];
                    $scope.selectedGameOtherTranslation.gameDescription = x.data.text[1];
                });
            };
            //$scope.Test = function (header,content) {

                
            //    integrationGamesService.Test(header,content).then(function (x) {
            //        console.log(x.data);
            //        $scope.selectedGameOtherTranslation = {};
            //        $scope.selectedGameOtherTranslation.gameName = x.data.text[0];
            //        $scope.selectedGameOtherTranslation.gameDescription = x.data.text[1];
            //    }, function (x) {
            //        console.log(x.data);
            //    });
            //};
            //test

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
                $scope.resetSelectedGameOrTranslation(false, true);
            }
            // ********************

            //reset selected game or translation true is reset 
            $scope.resetSelectedGameOrTranslation = function (game, translation) {
                if (game == true)
                    $scope.selectedGame = '';
                if (translation == true)
                    $scope.selectedGameOtherTranslation = '';
            }

            //set mark in main game translations 
            $scope.setTranslation = function (translationStatus) {
                $scope.selectedGame.gameTranslations.forEach(function (x) {
                    if (x.language.id == $scope.languageForOtherTranslation.language.id) {
                        x.hasTranslation = translationStatus;
                        //when translation status is false then i know translation not exists
                        if (!translationStatus)
                            $scope.resetSelectedGameOrTranslation(false, true);
                    };

                });
            };
            // ********************

            //edit insert new translation
            $scope.saveOtherTranslation = function () {
                //var userNotification = ''
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
                    //mark translation
                    $scope.setTranslation(true);
                    $scope.getTranslationForGame();

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
            // ********************

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
                        $scope.resetSelectedGameOrTranslation(true, false);
                }
            }
            // ********************

            //edit integration games
            $scope.editGame = function (selectedGame) {                
                integrationGamesService.PutIntegrationGame(selectedGame).then(function (x) {
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.GameEdited,
                        type: 'success',
                    });
                }, function (x) {
                    //var userNotification = '';
                    if (x.data.message == "GameNotExists")
                        userNotification = $scope.translation.GameNotExists;
                    else if (x.data.message == "CriticalError")
                        userNotification = $scope.translation.CriticalError;
                    else
                        userNotification = X.data.message; // handled error
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                    });
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
            $scope.deleteTranslation = function (selectedGameOtherTranslation) {
                dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.close();
                integrationGamesService.DeleteIntegrationGame(selectedGameOtherTranslation).then(function (x) {
                    //mark translation false 
                    $scope.setTranslation(false);
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.TranlastionRemoved,
                        type: 'success',
                    });
                },
               function (x) {
                   var userNotification = '';
                   console.log(x.data.message);
                   if (x.data.message == "1")
                       userNotification = $scope.translation.TranslationNotRemoved + " " + $scope.translation.GameTranslationNotExists;
                   else if (x.data.message == "2")
                       userNotification = $scope.translation.TranslationNotRemoved + " " + $scope.translation.GameFeaturesTranslationNotExists;
                   else
                       userNotification = $scope.translation.TranslationNotRemoved + " " + x.data.message;
                   $.Notify({
                       caption: $scope.translation.Failure,
                       content: userNotification,
                       type: 'alert',
                   });
               });

            }

            $scope.deleteGame = function (selectedGame, removeAllTranslation) {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.close();
                if (removeAllTranslation)
                    selectedGame.temporarySeveralTranslationDelete = removeAllTranslation;
                integrationGamesService.DeleteIntegrationGame(selectedGame).then(function (x) {
                    $scope.resetSelectedGameOrTranslation(true, true);
                    $scope.getIntegrationGamesWithLanguages($scope.selectedLanguage);
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.GameRemoved,
                        type: 'success',
                    });
                    
                },
                function (x) {
                   // var userNotification = '';
                    if (x.data.message == "GameTranslationNotExists")
                        userNotification = $scope.translation.GameTranslationNotExists;
                    else if (x.data.message == "GameFeatureTranslationNotExists")
                        userNotification = $scope.translation.GameFeaturesTranslationNotExists;
                    else if (x.data.message == "CriticalError")
                        userNotification = $scope.translation.CriticalError;
                    else
                        userNotification = x.data.message; // handled error
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


            function initContent(language) {
                $scope.getIntegrationGamesWithLanguages(language);
                $scope.getGameFeatuesModelWithDetails(language);
            };
            //base init
            initContent($scope.selectedLanguage);
            //$scope.getIntegrationGamesWithLanguages($scope.selectedLanguage);
            //$scope.getGameFeatuesModelWithDetails($scope.selectedLanguage);
            // ********************

          
            //when language change 
            $scope.$on('languageChange', function () {
                initContent(sharedService.sharedmessage);
                //$scope.getIntegrationGamesWithLanguages(sharedService.sharedmessage);
                //$scope.getGameFeatuesModelWithDetails(sharedService.sharedmessage);
            });
            // ********************
        }]);
})();
