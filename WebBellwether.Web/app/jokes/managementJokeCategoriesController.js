(function () {
    angular
        .module('webBellwether')
        .controller('managementJokeCategoriesController', ['$scope', '$timeout', 'sharedService', 'jokesService', function ($scope, $timeout, sharedService, jokesService) {
            var userNotification = '';
            //pagination and set other joke category translation (get from api)
            $scope.currentPage = 0;
            $scope.pageSize = 10;
            $scope.numberOfPages = function () {
                if ($scope.jokeCategories.length > 0)
                    return Math.ceil($scope.jokeCategories.length / $scope.pageSize);
                else
                    return 0;
            }
            $scope.selectedJokeCategory = '';
            $scope.selectedJokeCategoryTranslation = '';
            $scope.languageForOtherTranslation = '';
            $scope.setJokeCategory = function (selected) {
                if (selected.id == $scope.selectedJokeCategory.id) {
                    $scope.resetSelectedJokeCategoryOrTranslation(true, true);
                } else {
                    $scope.selectedJokeCategory = selected;
                    $scope.getTranslationForJokeCategory();
                }
            };

            $scope.getTranslationForJokeCategory = function () {
                //check language on new translation
                if ($scope.languageForOtherTranslation.language != null) {
                    $scope.selectedJokeCategory.jokeCategoryTranslations.forEach(function (x) {
                        if (x.language.id == $scope.languageForOtherTranslation.language.id && x.hasTranslation) {
                            $scope.selectedJokeCategoryTranslation.id = $scope.selectedJokeCategory.id;
                            jokesService.GetJokeCategoryTranslation($scope.selectedJokeCategory.id, $scope.languageForOtherTranslation.language.id).then(function (z) {
                                $scope.selectedJokeCategoryTranslation = x.data;
                            });
                            return;
                        }
                    });
                }
            };
            //edit joke category
            $scope.editJokeCategory = function (selectedJokeCategory) {
                jokesService.PutJokeCategory(selectedJokeCategory).then(function (x) {
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.JokeCategoryEdited,
                        type: 'success',
                    });
                },
                function (x) {
                    if (x.data.message == "JokeCategoryNotExists")
                        userNotification = $scope.translation.JokeCategoryNotExists;
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


            //delete joke category
            $scope.deleteDialog = function () {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.open();
            };
            $scope.countJokeCategoryTranslation = function (selectedJokeCategory) {
                var result = 0;
                selectedJokeCategory.jokeCategoryTranslations.forEach(function (x) {
                    if (x.hasTranslation)
                        result++;
                })
                return result;
            };
            $scope.deleteJokeCategory = function (selectedJokeCategory, removeAllTranslation) {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.close();
                if(removeAllTranslation)
                    selectedJokeCategory.temporarySeveralTranslationDelete = removeAllTranslation;
                jokesService.DeleteJokeCategory(selectedJokeCategory).then(function (x) {
                    $scope.resetSelectedJokeCategoryOrTranslation(true, true);
                    $scope.getJokeCategoriesWithAvailableLanguage($scope.selectedLanguage);
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.JokeCategoryDeleted,
                        type: 'success',
                    });
                },
                function (x) {
                    if (x.data.message == "JokeCategoryTranslationNotExists")
                        userNotification = $scope.translation.JokeCategoryTranslationNotExists;
                    else if (x.data.message == "CriticalError")
                        userNotification = $scope.translation.CriticalError;
                    else
                        userNotification = x.data.message; // handled error

                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                    });
                });
            };

            // ********************

            //add new category
            $scope.addJokeCategory = function (newJokeCategory, selectedLanguage) {
                var jokeCategory = {
                    Id: '',
                    JokeCategoryId: '',
                    JokeCategoryName: newJokeCategory,
                    LanguageId: selectedLanguage,
                    JokeCategoryTranslations: []
                }
                jokesService.PostJokeCategory(jokeCategory).then(function (x) {
                    initContent(selectedLanguage);
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.JokeCategoryAdded,
                        type: 'success',
                    });
                },
                function (x) {
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: $scope.translation.JokeCategoryNotAdded,
                        type: 'alert',
                    });
                });
            };
            // ********************

            //joke categories
            $scope.jokeCategories = [];
            $scope.getJokeCategoriesWithAvailableLanguage = function (lang) {
                jokesService.GetJokeCategoriesWithAvailableLanguage(lang).then(function (x) {
                    $scope.jokeCategories = [];
                    $scope.jokeCategories = x.data;
                });
            };
            $scope.setJokeCategoryTranslationAfterLanguageChange = function () {
                if ($scope.selectedJokeCategory != '') {
                    $scope.jokeCategories.forEach(function (o) {
                        if (o.id == $scope.selectedJokeCategory.id && o.languageId == $scope.selectedLanguage) {
                            $scope.selectedJokeCategory = o;
                            return;
                        }
                    });
                    if ($scope.selectedJokeCategory.languageId != $scope.selectedLanguage)
                        $scope.resetSelectedJokeCategoryOrTranslation(true, false);
                }
            };
            //reset selected game or translation true is reset 
            $scope.resetSelectedJokeCategoryOrTranslation = function (category, translation) {
                if (category == true)
                    $scope.selectedJokeCategory = '';
                if (translation == true)
                    $scope.selectedJokeCategoryTranslation = '';
            }
            // ********************

            function initContent(language) {
                $scope.getJokeCategoriesWithAvailableLanguage(language);
            };
            //base init
            initContent($scope.selectedLanguage);
            // ********************

            //when language change 
            $scope.$on('languageChange', function () {
                initContent(sharedService.sharedmessage);
            });

            // ********************
        }]);
})();
