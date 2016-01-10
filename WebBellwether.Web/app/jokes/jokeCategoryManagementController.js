(function () {
    angular
        .module('webBellwether')
        .controller('jokeCategoryManagementController', ['$scope', '$timeout', 'sharedService', 'jokeService', 'translateService', function ($scope, $timeout, sharedService, jokeService, translateService) {
            var userNotification = '';
            //pagination and set other joke category translation (get from api)
            $scope.currentPage = 0;
            $scope.pageSize = 10;
            $scope.numberOfPages = function () {
                if ($scope.jokeCategories == null)
                    return 0;
                if ($scope.jokeCategories.length > 0)
                    return Math.ceil($scope.jokeCategories.length / $scope.pageSize);
                else
                    return 0;
            }
            $scope.selectedJokeCategory = '';
            $scope.selectedJokeCategoryTranslation = '';
            $scope.newJokeCategory = '';
            $scope.languageForOtherTranslation = '';
            $scope.setJokeCategory = function (selected) {
                if (selected.Id === $scope.selectedJokeCategory.Id) {
                    $scope.resetSelectedJokeCategoryOrTranslation(true, true);
                } else {
                    $scope.selectedJokeCategory = selected;
                    $scope.getTranslationForJokeCategory();
                }
            };

            $scope.getTranslationForJokeCategory = function () {
                //check language on new translation
                if ($scope.languageForOtherTranslation != null) {
                    $scope.selectedJokeCategory.JokeCategoryTranslations.forEach(function (x) {
                        if (x.Language.Id === $scope.languageForOtherTranslation.Id && x.HasTranslation) {
                            $scope.selectedJokeCategoryTranslation.Id = $scope.selectedJokeCategory.Id;
                            jokeService.GetJokeCategoryTranslation($scope.selectedJokeCategory.Id, $scope.languageForOtherTranslation.Id).then(function (z) {
                                $scope.selectedJokeCategoryTranslation = z.Data;
                            });
                            return;
                        }
                    });
                }
                $scope.resetSelectedJokeCategoryOrTranslation(false, true);
            };
            //edit joke category
            $scope.editJokeCategory = function (selectedJokeCategory) {
                jokeService.PutJokeCategory(selectedJokeCategory).then(function (x) {
                    if (x.IsValid)
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.JokeCategoryEdited,
                            type: 'success'
                        });
                    else {
                        userNotification = validateErrorMessage('JokeCategoryNotEdited', x);
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


            //delete joke category
            $scope.deleteDialog = function () {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.open();
            };
            $scope.countJokeCategoryTranslation = function (selectedJokeCategory) {
                var result = 0;
                selectedJokeCategory.JokeCategoryTranslations.forEach(function (x) {
                    if (x.HasTranslation)
                        result++;
                });
                return result;
            };
            $scope.deleteJokeCategory = function (selectedJokeCategory, removeAllTranslation) {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.close();
                if (removeAllTranslation)
                    selectedJokeCategory.TemporarySeveralTranslationDelete = removeAllTranslation;
                jokeService.DeleteJokeCategory(selectedJokeCategory).then(function (x) {
                    if (x.IsValid) {
                        angular.forEach($scope.jokeCategories, function (x, index) {
                            if (x.Id === $scope.selectedJokeCategory.Id) {
                                $scope.jokeCategories.splice(index, 1);
                            }
                        });
                        $scope.resetSelectedJokeCategoryOrTranslation(true, true);
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.JokeCategoryDeleted,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('JokeCategoryNotDeleted',x);
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
            $scope.deleteTranslation = function (selectedJokeCategoryTranslation) {
                var dialog = $('#deleteTranslationDialog').data('dialog');
                dialog.close();
                jokeService.DeleteJokeCategory(selectedJokeCategoryTranslation).then(function (x) {
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

            //add new category
            $scope.addJokeCategory = function (newJokeCategory, selectedLanguage) {
                var jokeCategory = {
                    Id: '',
                    JokeCategoryId: '',
                    JokeCategoryName: newJokeCategory,
                    LanguageId: selectedLanguage,
                    JokeCategoryTranslations: []
                }
                jokeService.PostJokeCategory(jokeCategory).then(function (x) {
                    if (x.IsValid) {
                        if ($scope.jokeCategories === null)
                            $scope.jokeCategories = [];
                        $scope.jokeCategories.push(x.Data);
                        $scope.newJokeCategory = '';
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.JokeCategoryAdded,
                            type: 'success'
                        });
                    }
                    else {
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: $scope.translation.JokeCategoryNotAdded,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            // ********************

            //set mark in main joke category translations
            $scope.setTranslation = function (translationStatus) {
                $scope.selectedJokeCategory.JokeCategoryTranslations.forEach(function (x) {
                    if (x.Language.Id === $scope.languageForOtherTranslation.Id) {
                        x.HasTranslation = translationStatus;
                        if (!translationStatus)
                            $scope.resetSelectedJokeCategoryOrTranslation(false, true);
                    };
                });
            };

            //edit insert new translation 
            $scope.saveOtherJokeCategoryTranslation = function () {
                var jokeCategory = {
                    Id: $scope.selectedJokeCategory.Id,
                    JokeCategoryId: $scope.selectedJokeCategoryTranslation.JokeCategoryId,
                    JokeCategoryName: $scope.selectedJokeCategoryTranslation.JokeCategoryName,
                    LanguageId: $scope.languageForOtherTranslation.Id
                };
                jokeService.PostJokeCategory(jokeCategory).then(function(x) {
                    if (x.IsValid) {
                        $scope.setTranslation(true);
                        $scope.getTranslationForJokeCategory();

                        if (jokeCategory.JokeCategoryId == null) {
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
                        if (jokeCategory.jokeCategoryId == null) {
                            userNotification = $scope.translation.TranslationNotAdded;
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


            //joke categories
            $scope.jokeCategories = [];
            $scope.getJokeCategoriesWithAvailableLanguage = function (lang) {
                jokeService.GetJokeCategoriesWithAvailableLanguage(lang).then(function (x) {
                    $scope.jokeCategories = [];
                    $scope.jokeCategories = x.Data;
                    $scope.setJokeCategoryTranslationAfterLanguageChange();
                });
            };
            $scope.setJokeCategoryTranslationAfterLanguageChange = function () {
                if ($scope.selectedJokeCategory !== '') {
                    $scope.jokeCategories.forEach(function (o) {
                        if (o.Id === $scope.selectedJokeCategory.Id && o.LanguageId === $scope.selectedLanguage) {
                            $scope.selectedJokeCategory = o;
                            return;
                        }
                    });
                    if ($scope.selectedJokeCategory.LanguageId !== $scope.selectedLanguage)
                        $scope.resetSelectedJokeCategoryOrTranslation(true, false);
                }
            };
            //reset selected game or translation true is reset 
            $scope.resetSelectedJokeCategoryOrTranslation = function (category, translation) {
                if (category === true)
                    $scope.selectedJokeCategory = '';
                if (translation === true)
                    $scope.selectedJokeCategoryTranslation = '';
            }
            // ********************

            //translate joke cateogory
            $scope.translateJokeCategory = function (selectedJokeCategory, targetJokeCategoryLanguage) {
                var selectedJokeCategoryLanguageCode = '';
                $scope.languages.forEach(function (x) {
                    if (x.Id === selectedJokeCategory.LanguageId)
                        selectedJokeCategoryLanguageCode = x.LanguageShortName;
                });
                var translateLanguageModel = {
                    CurrentLanguageCode: selectedJokeCategoryLanguageCode,
                    TargetLanguageCode: targetJokeCategoryLanguage.LanguageShortName,
                    ContentForTranslation: [selectedJokeCategory.JokeCategoryName]
                }
                var selectedJokeCategoryTranslationCategoryIdIfExists = $scope.selectedJokeCategoryTranslation.JokeCategoryId;
                translateService.PostLanguageTranslation(translateLanguageModel).then(function(x) {
                    if (x.IsValid) {
                        $scope.selectedJokeCategoryTranslation = {
                            JokeCategoryId: selectedJokeCategoryTranslationCategoryIdIfExists,
                            JokeCategoryName: x.Data.text[0]
                        };
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.TranslationCompletedSuccessfully,
                            type: 'success'
                        });
                    } else {
                        userNotification = $scope.translation.ErrorDuringTranslation + ' ' + x.Data.message;
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
                    $scope.getJokeCategoriesWithAvailableLanguage(language);
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
