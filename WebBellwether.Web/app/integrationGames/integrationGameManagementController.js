(function () {
    angular
        .module('webBellwether')
        .controller('integrationGameManagementController', ['$scope', '$timeout', 'integrationGameService', 'translateService', 'sharedService', function ($scope, $timeout, integrationGameService, translateService, sharedService) {
            var userNotification = '';
            //pagination
            $scope.currentPage = 0;
            $scope.pageSize = 10;
            $scope.numberOfPages = function() {
                if ($scope.integrationGames == null)
                    return 0;
                if ($scope.integrationGames.length > 0)
                    return Math.ceil($scope.integrationGames.length / $scope.pageSize);
                else
                    return 0;
            };
            // ********************   

            $scope.selectedGame = '';
            $scope.selectedGameTranslation = '';
            $scope.languageForOtherTranslation = '';
            $scope.setSelectGame = function (selected) {
                if (selected.Id === $scope.selectedGame.Id) {
                    $scope.resetSelectedGameOrTranslation(true, true);
                } else {
                    $scope.selectedGame = selected;
                    $scope.getTranslationForGame();
                }
            };

            $scope.getTranslationForGame = function () {
                //check language on new translation 
                if ($scope.languageForOtherTranslation != null) {
                    //check available translation by language in selectedGame
                    $scope.selectedGame.GameTranslations.forEach(function (o) {
                        if (o.Language.Id === $scope.languageForOtherTranslation.Id && o.HasTranslation) {
                            $scope.selectedGameTranslation.Id = $scope.selectedGame.Id;
                            integrationGameService.GetIntegrationGameTranslation($scope.selectedGame.Id, $scope.languageForOtherTranslation.Id).then(function (x) {
                                $scope.selectedGameTranslation = x.Data;
                            });
                            return;
                        }
                    });
                }
                $scope.resetSelectedGameOrTranslation(false, true);
            };

            //reset selected game or translation true is reset 
            $scope.resetSelectedGameOrTranslation = function (game, translation) {
                if (game === true)
                    $scope.selectedGame = '';
                if (translation === true)
                    $scope.selectedGameTranslation = '';
            }

            //set mark in main game translations 
            $scope.setTranslation = function (translationStatus) {
                $scope.selectedGame.GameTranslations.forEach(function (x) {
                    if (x.Language.Id === $scope.languageForOtherTranslation.Id) {
                        x.HasTranslation = translationStatus;
                        //when translation status is false then i know translation not exists
                        if (!translationStatus)
                            $scope.resetSelectedGameOrTranslation(false, true);
                    };

                });
            };
            // ********************

            //edit insert new translation
            $scope.saveOtherTranslation = function () {
                var integrationGame = {
                    Id: $scope.selectedGame.Id,//general id 
                    IntegrationGameId: $scope.selectedGameTranslation.IntegrationGameId, // id for translation
                    GameName: $scope.selectedGameTranslation.GameName,
                    GameDetails: $scope.selectedGameTranslation.GameDescription,
                    Language: $scope.languageForOtherTranslation.Id,
                    Features: []
                }
                $scope.selectedGame.IntegrationGameDetailModels.forEach(function (x) {
                    integrationGame.Features.push(x.GameFeatureDetailId);
                });
                integrationGameService.AddNewIntegrationGame(integrationGame).then(function(x) {
                    if (x.IsValid) {
                        //mark translation
                        $scope.setTranslation(true);
                        $scope.getTranslationForGame();

                        if (integrationGame.IntegrationGameId == null | integrationGame.IntegrationGameId == undefined) {
                            userNotification = $scope.translation.TranslationAdded;
                        } else {
                            userNotification = $scope.translation.TranslationEdited;
                        }
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: userNotification,
                            type: 'success'
                        });
                    } else {
                        if (integrationGame.IntegrationGameId == undefined | integrationGame.IntegrationGameId == null) {
                            userNotification = validateErrorMessage('TranslationNotAdded',x) + " " + $scope.translation.ForLanguage + $scope.languageForOtherTranslation.LanguageName + " : " + $scope.languageForOtherTranslation.LanguageShortName;
                        } else
                            userNotification = $scope.translation.TranslationNotEdited;
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            // ********************

            //new integration game
            $scope.newIntegrationGame = '';
            $scope.addIntegrationGame = function (newIntegrationGame, selectedLanguage) {
                var integrationGame = {
                    GameName: newIntegrationGame.GameName,
                    GameDetails: newIntegrationGame.GameDescription,
                    Language: selectedLanguage,
                    Features: []
                }
                angular.forEach(newIntegrationGame.Features, function (x) {
                    integrationGame.Features.push(x.GameFeatureDetailId);
                });
                console.log(integrationGame);
                integrationGameService.AddNewIntegrationGame(integrationGame).then(function(x) {
                    if (x.IsValid) {
                        if ($scope.integrationGames == null)
                            $scope.integrationGames = [];
                        $scope.integrationGames.push(x.Data);
                        $scope.newIntegrationGame = '';

                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.GameAdded,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('GameNotAdded', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert'
                        });
                    }

                });
            };

            //integration games
            $scope.integrationGames = [];
            $scope.getIntegrationGamesWithLanguages = function (lang) {
                integrationGameService.IntegrationGamesWithAvailableLanguages(lang).then(function (x) {
                    $scope.integrationGames = [];
                    $scope.integrationGames = x.Data;
                    $scope.setGameTranslationAfterLanguageChange();
                });
            };

            $scope.setGameTranslationAfterLanguageChange = function () {
                if ($scope.selectedGame !== '') {
                    $scope.integrationGames.forEach(function (o) {
                        if (o.Id === $scope.selectedGame.Id && o.Language.Id === $scope.selectedLanguage) {
                            $scope.selectedGame = o;
                            return;
                        }
                    });
                    if ($scope.selectedGame.Language.Id !== $scope.selectedLanguage)
                        $scope.resetSelectedGameOrTranslation(true, false);
                }
            }
            // ********************

            //edit integration games
            $scope.editGame = function (selectedGame) {
                integrationGameService.PutIntegrationGame(selectedGame).then(function(x) {
                    if (x.IsValid) {
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.GameEdited,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('GameNotEdited', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert'
                        });
                    }
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
                selectedGame.GameTranslations.forEach(function(x) {
                    if (x.HasTranslation)
                        result++;
                });
                return result;
            }
            $scope.deleteTranslationDialog = function () {
                var dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.open();
            }
            $scope.deleteTranslation = function (selectedGameTranslation) {
                var dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.close();
                integrationGameService.DeleteIntegrationGame(selectedGameTranslation).then(function(x) {
                    if (x.IsValid) {
                        //mark translation false 
                        $scope.setTranslation(false);
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.TranlastionRemoved,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('TranslationNotRemoved', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert'
                        });
                    }

                });
            }

            $scope.deleteGame = function (selectedGame, removeAllTranslation) {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.close();
                if (removeAllTranslation)
                    selectedGame.TemporarySeveralTranslationDelete = removeAllTranslation;
                integrationGameService.DeleteIntegrationGame(selectedGame).then(function(x) {
                    if (x.IsValid) {
                        $scope.resetSelectedGameOrTranslation(true, true);
                        $scope.getIntegrationGamesWithLanguages($scope.selectedLanguage);
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.GameRemoved,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('GameNotRemoved', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert'
                        });
                    }
                });
            };
            // ********************

            //game features 
            $scope.gameFeatures = [];
            $scope.getGameFeatuesModelWithDetails = function (lang) {
                integrationGameService.GameFeatuesModelWithDetails(lang).then(function (x) {
                    $scope.gameFeatures = x.Data;
                });
            };
            // ********************

            //translate game 
            //selectedGame.language.id,languageForOtherTranslation.language.id,selectedGame.gameName,selectedGame.gameDescription
            $scope.translateGame = function (currentLanguage, targetLanguage, gameName, gameDescription) {
                var translateLanguageModel = {
                    CurrentLanguageCode: currentLanguage.LanguageShortName,
                    TargetLanguageCode: targetLanguage.LanguageShortName,
                    ContentForTranslation: [gameName, gameDescription]
                };
                var selectedGameTranslationIdIfExists = $scope.selectedGameTranslation.IntegrationGameId;
                translateService.PostLanguageTranslation(translateLanguageModel).then(function(x) {
                    if (x.IsValid) {
                        $scope.selectedGameTranslation = {
                            GameName: x.Data.text[0],
                            GameDescription: x.Data.text[1],
                            IntegrationGameId: selectedGameTranslationIdIfExists,
                            Id: $scope.selectedGame.Id
                        };

                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.TranslationCompletedSuccessfully,
                            type: 'success'
                        });
                    } else {
                        userNotification = $scope.translation.ErrorDuringTranslation + ' ' + x.ErrorMessage;
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            function validateErrorMessage(header, x) {
                if ($scope.translation[x.ErrorMessage] === undefined)
                    return $scope.translation[header] + ' ' + x.ErrorMessage;
                else
                    return $scope.translation[header] + ' ' + $scope.translation[x.ErrorMessage];
            }

            //base init
            function initContent(language) {
                if (language !== undefined) {
                    $scope.getIntegrationGamesWithLanguages(language);
                    $scope.getGameFeatuesModelWithDetails(language);
                }
            };
            initContent($scope.selectedLanguage);
            // ********************


            //when language change 
            $scope.$on('languageChange', function () {
                initContent(sharedService.sharedmessage);
            });
            // ********************
        }]);
})();
