(function () {
    angular
        .module('webBellwether')
        .controller('managementJokesController', ['$scope', '$timeout', 'sharedService', 'jokesService', function ($scope, $timeout, sharedService, jokesService) {
            var userNotification = '';
            //pagination and set other joke category translation (get from api)
            $scope.currentPage = 0;
            $scope.pageSize = 10;
            $scope.numberOfPages = function () {
                if ($scope.jokes.length > 0)
                    return Math.ceil($scope.jokes.length / $scope.pageSize);
                else
                    return 0;
            };

            $scope.selectedJoke = '';
            $scope.selectedJokeTranslation = '';
            $scope.languageForOtherTranslation = '';
            $scope.setSelectedJoke = function (selected) {
                if (selected.id == $scope.selectedJoke.id)
                    $scope.resetSelectedJokeOrTranslation(true, true);
                else {
                    $scope.selectedJoke = selected;
                    $scope.getTranslationForJoke();
                }
            };
            $scope.resetSelectedJokeOrTranslation = function (joke, translation) {
                if (joke == true)
                    $scope.selectedJoke = '';
                if (translation == true)
                    $scope.selectedJokeTranslation = '';
            };


            $scope.getTranslationForJoke = function () {
                //check language on new translation
                if ($scope.languageForOtherTranslation.language != null) {
                    $scope.selectedJoke.jokeTranslations.forEach(function (x) {
                        if (x.language.id == $scope.languageForOtherTranslation.language.id && x.hasTranslation) {
                            $scope.selectedJokeTranslation.id = $scope.selectedJoke.id;
                            jokesService.GetJokeTranslation($scope.selectedJoke.id, $scope.languageForOtherTranslation.language.id).then(function (z) {
                                $scope.selectedJokeTranslation = z.data;
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
                selectedJoke.jokeTranslations.forEach(function (x) {
                    if (x.hasTranslation)
                        result++;
                })
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
                    selectedJoke.temporarySeveralTranslationDelete = removeAllTranslation;
                jokesService.DeleteJoke(selectedJoke).then(function (x) {
                    angular.forEach($scope.jokes, function (x, index) {
                        if (x.id == $scope.selectedJoke.id) {
                            $scope.jokes.splice(index, 1);
                        }
                    });
                    $scope.resetSelectedJokeOrTranslation(true, true);
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.JokeDeleted,
                        type: 'success',
                    });
                },
                function (x) {
                    userNotification = $scope.translation.JokeNotDeleted + ' ' + $scope.translation[x.data.message];
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                    });
                });
            };
            $scope.deleteTranslationDialog = function () {
                var dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.open();
            };
            $scope.deleteTranslation = function (selectedJokeTranslation) {
                dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.close();
                jokesService.DeleteJoke(selectedJokeTranslation).then(function (x) {
                    $scope.setTranslation(false);
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.TranlastionRemoved,
                        type: 'success',
                    });
                },
                function (x) {
                    userNotification = $scope.translation.TranslationNotRemoved + $scope.translation[x.data.message];
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                    });
                });
            };

            // ********************

            //add new joke
            $scope.newJoke = '';
            $scope.addJoke = function (newjoke, language) {
                var joke = {
                    Id: '',
                    JokeId: '',
                    JokeContent: newjoke.jokeContent,
                    LanguageId: language,
                    JokeCategoryId: newjoke.jokeCategoryId
                };
                jokesService.PostJoke(joke).then(function (x) {
                    $scope.jokes.push(x.data);
                    $scope.newJoke = '';
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.JokeAdded,
                        type: 'success',
                    });
                },
                function (x) {
                    userNotification = $scope.translation.JokeNotAdded + ' ' + $scope.translation[x.data.message];
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                        timeout: 10000
                    });
                });
            };

            //*******************


            //edit joke
            $scope.editJoke = function (selectedJoke) {
                jokesService.PutJoke(selectedJoke).then(function (x) {
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.JokeCategoryEdited,
                        type: 'success',
                    });
                }, function (x) {
                    userNotification = $scope.translation.TranslationNotEdited;
                    if (x.data.message.indexOf(",") > -1) {
                        userNotification += " " + $scope.translation.JokeCategoryNotExists + " " + $scope.translation.ForLanguage + " "  + x.data.message.substr(x.data.message.indexOf(",") + 1);
                    } else {
                        userNotification + $scope.translation[x.data.message];
                    }
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                        timeout:10000
                    });                   
                });
            };
            //*******************

            //set mark in main joke category translations
            $scope.setTranslation = function (translationStatus) {
                $scope.selectedJoke.jokeTranslations.forEach(function (x) {
                    if (x.language.id == $scope.languageForOtherTranslation.language.id) {
                        x.hasTranslation = translationStatus;
                        if (!translationStatus)
                            $scope.resetSelectedJokeOrTranslation(false, true);
                    };
                });
            };
            //*******************

            //edit insert new translation
            $scope.saveOtherJokeTranslation = function () {
                var joke = {
                    Id: $scope.selectedJoke.id,
                    JokeId: $scope.selectedJokeTranslation.jokeId,
                    JokeContent: $scope.selectedJokeTranslation.jokeContent,
                    LanguageId: $scope.languageForOtherTranslation.language.id,
                    JokeCategoryId: $scope.selectedJoke.jokeCategoryId
                };
                jokesService.PostJoke(joke).then(function (x) {
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
                        type: 'success',
                    });
                }, function (x) {
                    if (joke.JokeId == null) {
                        userNotification = $scope.translation.TranslationNotAdded;
                    }
                    else
                        userNotification = $scope.translation.TranslationNotEdited;

                    if (x.data.message.indexOf(",") > -1) {
                        userNotification += " " + $scope.translation.JokeCategoryNotExists + " " + $scope.translation.ForLanguage + " " + x.data.message.substr(x.data.message.indexOf(",") + 1);
                    } else {
                        userNotification += " " + $scope.translation[x.data.message];
                    }
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                        timeout: 10000
                    });
                });
            };
            //*******************

            //jokes
            $scope.jokes = [];
            $scope.getJokeWithAvailableLanguages = function (lang) {
                jokesService.GetJokeWithAvailableLanguages(lang).then(function (x) {
                    $scope.jokes = [];
                    $scope.jokes = x.data;
                });
            };
            //*******************

            //joke categories
            $scope.jokeCategories = [];
            $scope.getJokeCategories = function (lang) {
                jokesService.GetJokeCategories(lang).then(function (x) {
                    $scope.jokeCategories = [];
                    $scope.jokeCategories = x.data;
                });
            };
            //*******************

            //init content
            function initContent(language) {
                $scope.getJokeCategories(language);
                $scope.getJokeWithAvailableLanguages(language);
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