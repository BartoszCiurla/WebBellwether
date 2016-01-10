(function () {
    angular
        .module('webBellwether')
        .controller('jokeManagementController', ['$scope', '$timeout', 'sharedService', 'translateService', 'jokeService', function ($scope, $timeout, sharedService, translateService, jokeService) {
            var userNotification = '';
            //pagination and set other joke category translation (get from api)
            $scope.currentPage = 0;
            $scope.pageSize = 10;
            $scope.numberOfPages = function () {
                if ($scope.jokes == null)
                    return 0;
                if ($scope.jokes.length > 0)
                    return Math.ceil($scope.jokes.length / $scope.pageSize);
                else
                    return 0;
            };

            $scope.selectedJoke = '';
            $scope.selectedJokeTranslation = '';
            $scope.languageForOtherTranslation = '';
            $scope.setSelectedJoke = function (selected) {
                if (selected.Id === $scope.selectedJoke.Id)
                    $scope.resetSelectedJokeOrTranslation(true, true);
                else {
                    $scope.selectedJoke = selected;
                    $scope.getTranslationForJoke();
                }
            };
            $scope.resetSelectedJokeOrTranslation = function (joke, translation) {
                if (joke === true)
                    $scope.selectedJoke = '';
                if (translation === true)
                    $scope.selectedJokeTranslation = '';
            };


            $scope.getTranslationForJoke = function () {
                //check language on new translation
                if ($scope.languageForOtherTranslation != null) {
                    $scope.selectedJoke.JokeTranslations.forEach(function (x) {
                        if (x.Language.Id === $scope.languageForOtherTranslation.Id && x.HasTranslation) {
                            $scope.selectedJokeTranslation.Id = $scope.selectedJoke.Id;
                            jokeService.GetJokeTranslation($scope.selectedJoke.Id, $scope.languageForOtherTranslation.Id).then(function (z) {
                                $scope.selectedJokeTranslation = z.Data;
                            });
                            return;
                        }
                    });
                }
                $scope.resetSelectedJokeOrTranslation(false, true);
            };
            //*******************

            //delete joke
            $scope.countJokeTranslation = function (selectedJoke) {
                var result = 0;
                selectedJoke.JokeTranslations.forEach(function(x) {
                    if (x.HasTranslation)
                        result++;
                });
                return result;
            };
            $scope.deleteDialog = function () {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.open();
            };

            $scope.deleteJoke = function (selectedJoke, removeAllTranslation) {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.close();
                if (removeAllTranslation)
                    selectedJoke.TemporarySeveralTranslationDelete = removeAllTranslation;
                jokeService.DeleteJoke(selectedJoke).then(function(x) {
                    if (x.IsValid) {
                        angular.forEach($scope.jokes, function(x, index) {
                            if (x.Id === $scope.selectedJoke.Id) {
                                $scope.jokes.splice(index, 1);
                            }
                        });
                        $scope.resetSelectedJokeOrTranslation(true, true);
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.JokeDeleted,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('JokeNotDeleted', x);
                        //if ($scope.translation[x.ErrorMessage] === undefined)
                        //    return $scope.translation.JokeNotDeleted + ' ' + x.ErrorMessage;
                        //else
                        //    return $scope.translation.JokeNotDeleted + ' ' + $scope.translation[x.ErrorMessage];
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };        

            $scope.deleteTranslationDialog = function () {
                var dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.open();
            };
            $scope.deleteTranslation = function (selectedJokeTranslation) {
                var dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.close();
                jokeService.DeleteJoke(selectedJokeTranslation).then(function(x) {
                    if (x.IsValid) {
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
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            // ********************

            //add new joke
            $scope.newJoke = '';
            $scope.addJoke = function (newjoke, language) {
                var joke = {
                    Id: '',
                    JokeId: '',
                    JokeContent: newjoke.JokeContent,
                    LanguageId: language,
                    JokeCategoryId: newjoke.JokeCategoryId
                };
                jokeService.PostJoke(joke).then(function(x) {
                    if (x.IsValid) {
                        if ($scope.jokes === null)
                            $scope.jokes = [];
                        $scope.jokes.push(x.Data);
                        $scope.newJoke = '';
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.JokeAdded,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('JokeNotAdded', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            //*******************


            //edit joke
            $scope.editJoke = function (selectedJoke) {
                jokeService.PutJoke(selectedJoke).then(function(x) {
                    if (x.IsValid) {
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.JokeCategoryEdited,
                            type: 'success'
                        });
                    } else {
                        userNotification = $scope.translation.TranslationNotEdited;
                        if (x.ErrorMessage.indexOf(",") > -1) {
                            userNotification += " " + $scope.translation.JokeCategoryNotExists + " " + $scope.translation.ForLanguage + " " + x.ErrorMessage.substr(x.ErrorMessage.indexOf(",") + 1);
                        } else {
                            userNotification += $scope.translation[x.ErrorMessage];
                        }
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            //*******************

            //set mark in main joke category translations
            $scope.setTranslation = function (translationStatus) {
                $scope.selectedJoke.JokeTranslations.forEach(function (x) {
                    if (x.Language.Id === $scope.languageForOtherTranslation.Id) {
                        x.HasTranslation = translationStatus;
                        if (!translationStatus)
                            $scope.resetSelectedJokeOrTranslation(false, true);
                    };
                });
            };
            //*******************

            //edit insert new translation
            $scope.saveOtherJokeTranslation = function () {
                var joke = {
                    Id: $scope.selectedJoke.Id,
                    JokeId: $scope.selectedJokeTranslation.JokeId,
                    JokeContent: $scope.selectedJokeTranslation.JokeContent,
                    LanguageId: $scope.languageForOtherTranslation.Id,
                    JokeCategoryId: $scope.selectedJoke.JokeCategoryId
                };
                jokeService.PostJoke(joke).then(function(x) {
                    if (x.IsValid) {
                        $scope.setTranslation(true);
                        $scope.getTranslationForJoke();
                        if (joke.JokeId == null) {
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
                        if (joke.JokeId == null) {
                            userNotification = $scope.translation.TranslationNotAdded;
                        } else
                            userNotification = $scope.translation.TranslationNotEdited;

                        if (x.ErrorMessage.indexOf(",") > -1) {
                            userNotification += " " + $scope.translation.JokeCategoryNotExists + " " + $scope.translation.ForLanguage + " " + x.ErrorMessage.substr(x.ErrorMessage.indexOf(",") + 1);
                        } else {
                            userNotification += " " + $scope.translation[x.ErrorMessage];
                        }
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            //*******************

            //jokes
            $scope.jokes = [];
            $scope.getJokeWithAvailableLanguages = function (lang) {
                jokeService.GetJokeWithAvailableLanguages(lang).then(function (x) {
                    $scope.jokes = [];
                    $scope.jokes = x.Data;
                });
            };
            //*******************

            //joke categories
            $scope.jokeCategories = [];
            $scope.getJokeCategories = function (lang) {
                jokeService.GetJokeCategories(lang).then(function (x) {
                    $scope.jokeCategories = [];
                    $scope.jokeCategories = x.Data;
                });
            };
            //*******************

            //translate joke 
            $scope.translateJoke = function(selectedJoke,targetJokeLanguage) {
                var selectedJokeLanguageCode = '';
                $scope.languages.forEach(function(x) {
                    if (x.Id === selectedJoke.LanguageId)
                        selectedJokeLanguageCode = x.LanguageShortName;
                });
                var translateLanguageModel = {
                    CurrentLanguageCode: selectedJokeLanguageCode,
                    TargetLanguageCode: targetJokeLanguage.LanguageShortName,
                    ContentForTranslation: [selectedJoke.JokeContent]
                };
                var selectedJokeTranslationIdIfExists = $scope.selectedJokeTranslation.JokeId;
                translateService.PostLanguageTranslation(translateLanguageModel).then(function(x) {
                    if (x.IsValid) {
                        $scope.selectedJokeTranslation = {
                            JokeContent: x.Data.Result.text[0],
                            JokeId: selectedJokeTranslationIdIfExists
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

            //init content
            function initContent(language) {
                if (language !== undefined) {
                    $scope.getJokeCategories(language);
                    $scope.getJokeWithAvailableLanguages(language);
                }
            };
            //base init
            initContent($scope.selectedLanguage);

            //when language change
            $scope.$on('languageChange', function () {
                initContent(sharedService.sharedmessage);
            });
            //*******************
        }]);
})();