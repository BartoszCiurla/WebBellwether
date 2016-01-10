(function () {
    angular
        .module('webBellwether')
        .controller('languageManagementController', ['$scope', '$timeout', 'languageService', 'translateService', function ($scope, $timeout, languageService, translateService) {
            var userNotification = '';
            $scope.currentPage = 0;
            $scope.pageSize = 20;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.selectedLanguageContent.length / $scope.pageSize);
            };
            $scope.goToTop = function () {
                window.scrollTo(0, 990);
            };
            $scope.activeLanguageKey = '';
            $scope.setActiveLanguageKey = function (active) {
                if (active.Key !== $scope.activeLanguageKey)
                    $scope.activeLanguageKey = active.Key;
                else {
                    $scope.activeLanguageKey = null;
                }
            };

            //search 
            $scope.searchSource = [];
            $scope.changeSearchSource = function () {
                $scope.searchSource = [];
                $scope.selectedLanguageContent.forEach(function (z) {
                    $scope.searchSource.push(z.Key);
                    $scope.searchSource.push(z.Value);
                });
            };
            //***********************

            //new language dialog
            $scope.newLanguageOption = false;
            $scope.newLanguageDialog = function () {
                if (!$scope.newLanguageOption) {
                    $scope.newLanguageOption = true;
                }
                else {
                    $scope.newLanguageOption = false;
                }
            }; //***********************

            //new language
            $scope.newLanguage = '';
            $scope.addNewLanguage = function (language) {
                languageService.PostLanguage(language).then(function(x) {
                    if (x.IsValid) {
                        $scope.allLanguages.push(x.Data);
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.LanguageAdded,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('LanguageNotAdded', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            //***********************

            //edit language key
            $scope.languageForEdit = null;
            $scope.editLanguageKey = function (selectedLanguageKey, lang) {
                var languageKey = {
                    Key: selectedLanguageKey.Key,
                    Value: selectedLanguageKey.Value,
                    LanguageId: lang.Id
                };
                languageService.PutLanguageKey(languageKey).then(function(x) {
                    if (x.IsValid) {
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.LanguageKeyValueEdited,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('LanguageKeyValueNotEdited', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            //***********************

            //edit language
            $scope.editLanguage = function (language) {
                languageService.PutLanguage(language).then(function(x) {
                    if (x.IsValid) {
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.LanguageEdited,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('LanguageNotEdited', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            //***********************


            //delete language
            $scope.deleteDialog = function () {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.open();
            };
            $scope.deleteLanguageFromAllList = function (language) {
                angular.forEach($scope.languages, function (z, index) {
                    if (z.Id === language.Id)
                        $scope.languages.splice(index, 1);
                });

                angular.forEach($scope.allLanguages, function (z, index) {
                    if (z.Id === language.Id)
                        $scope.allLanguages.splice(index, 1);
                });
                $scope.languageForEdit = null;
            };
            $scope.deleteLanguage = function (language) {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.close();
                console.log(language);
                languageService.DeleteLanguage(language).then(function(x) {
                    if (x.IsValid) {
                        $scope.deleteLanguageFromAllList(language);
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.LanguageRemoved,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('LanguageNotRemoved', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };

            //***********************

            //publish language
            $scope.languageHasBeenPublished = function (language, langExistsInCurrentList) {
                userNotification += ' ' + $scope.translation.LanguageHasBeenPublished;
                if (!langExistsInCurrentList) {
                    $scope.languages.push(language);
                }
            };
            $scope.languageHasBeenUnPublic = function (language) {
                userNotification += ' ' + $scope.translation.LanguageHasBeenNonPublic;
                angular.forEach($scope.languages, function (z, index) {
                    if (z.Id === language.Id)
                        $scope.languages.splice(index, 1);
                });
            };
            $scope.checkIsLangExistsInGlobalList = function (language) {
                var langExistsInCurrentList = false;
                $scope.languages.forEach(function (x) {
                    if (x.Id === language.Id)
                        langExistsInCurrentList = true;
                });
                return langExistsInCurrentList;
            };
            $scope.publishLanguage = function (language) {
                var langExistsInCurrentList = $scope.checkIsLangExistsInGlobalList(language);

                languageService.PutPublishLanguage(language).then(function(x) {
                    if (x.IsValid) {
                        userNotification = $scope.translation.LanguageEdited;
                        if (x.Data === "LanguageHasBeenPublished") {
                            $scope.languageHasBeenPublished(language, langExistsInCurrentList);
                        } else if (x.Data === "LanguageHasBeenNonpublic") {
                            $scope.languageHasBeenUnPublic(language);
                        }
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: userNotification,
                            type: 'success'
                        });
                    } else {
                        userNotification = validateErrorMessage('LanguageNotEdited', x);
                        language.IsPublic = !language.IsPublic;
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            //***********************

            //language content
            $scope.selectedLanguageForEdit = null;
            $scope.selectedLanguageContent = [];
            $scope.getLanguageFileContent = function(lang) {
                languageService.GetLanguageContent(lang.Id).then(function (x) {
                    $scope.selectedLanguageContent = [];
                    for (key in x) {
                        if (x.hasOwnProperty(key)) {
                            if (key.indexOf('$', 0) === -1 && key !== "toJSON")                                
                                $scope.selectedLanguageContent.push({ Key: key, Value: x[key] });
                        }
                    }
                    $scope.changeSearchSource();
                });
            }
            $scope.getLanguageContent = function (lang) {
                $scope.selectedLanguageContent = [];
                $scope.selectedLanguageForEdit = null;
                $scope.languageForEdit = null;
                if (lang != null) {
                    $scope.selectedLanguageForEdit = lang;
                    $scope.languageForEdit = {
                        Id: lang.Id,
                        LanguageName: lang.LanguageName,
                        LanguageShortName: lang.LanguageShortName,
                        IsPublic: lang.IsPublic,
                        LanguageVersion : lang.LanguageVersion
                    };
                    $scope.getLanguageFileContent(lang);
                }
            };
            // ********************

            //languages list
            $scope.allLanguages = [];
            $scope.getAllLanguages = function () {
                languageService.GetAllLanguages().then(function (x) {
                    $scope.allLanguages = x.Data;
                    $scope.allLanguages.forEach(function (z) {
                        if (z.Id === $scope.selectedLanguage)
                            $scope.getLanguageContent(z);
                    });
                });
            };
            // ********************

            //supported languages for translate service
            $scope.supportedLanguages = [];
            $scope.translateServiceName = '';
            $scope.getSupportedLanguages = function () {
                translateService.GetLanguages().then(function (x) {
                    $scope.supportedLanguages = x.Data;
                });
            };
            $scope.getTranslateServiceName = function () {
                translateService.GetTranslateServiceName().then(function (x) {
                    $scope.translateServiceName = x.Data;
                });
            };
            // ********************

            //translate function
            $scope.translateLanguageKey = function (currentLangId, targetLang, content, key) {
                var shortNameCurrentLang = '';
                $scope.languages.forEach((function (x) {
                    if (x.Id === currentLangId)
                        shortNameCurrentLang = x.LanguageShortName;
                }));
                var translateLanguageModel = {
                    CurrentLanguageCode: shortNameCurrentLang,
                    TargetLanguageCode: targetLang,
                    ContentForTranslation: [content]
                };
                translateService.PostLanguageTranslation(translateLanguageModel).then(function(x) {
                    if (x.IsValid) {
                        $scope.selectedLanguageContent.forEach(function(z) {
                            if (z.Key === key) {
                                z.Value = x.Data.Result.text[0];
                            }
                        });
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.TranslationCompletedSuccessfully + " " + x.Data.Result.text[0],
                            type: 'success',
                            timeout: 10000
                        });
                    } else {
                        userNotification = validateErrorMessage('ErrorDuringTranslation', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };

            $scope.translateAllLanguageKeysDialog = function () {
                var dialog = $('#translateAllDialog').data('dialog');
                dialog.open();
            };
            $scope.translateAllLangaugeKeysCloseDialog = function () {
                var dialog = $('#translateAllDialog').data('dialog');
                dialog.close();
            };
            $scope.translateAllLanguageKeys = function (currentLangId, targetLang) {
                var selectedLangauge = '';
                $scope.languages.forEach((function (x) {
                    if (x.Id === currentLangId)
                        selectedLangauge = x;
                }));
                var translateLangaugeKeysModel = {
                    CurrentLanguageShortName: selectedLangauge.LanguageShortName,
                    CurrentLanguageId: selectedLangauge.Id,
                    TargetLangaugeShortName: targetLang.LanguageShortName,
                    TargetLanguageId: targetLang.Id
                };
                translateService.PostTranslateAllLanguageKeys(translateLangaugeKeysModel).then(function(x) {
                    if (x.IsValid) {
                        $scope.getLanguageFileContent(targetLang);
                        $scope.translateAllLangaugeKeysCloseDialog();
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.KeyValuesAreCorrectlyFilled,
                            type: 'success',
                            timeout: 10000
                        });
                    } else {
                        $scope.translateAllLangaugeKeysCloseDialog();
                        userNotification = validateErrorMessage('KeyValuesAreNotFilled', x);
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: userNotification,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };

            $scope.translateLanguageName = function(language, languageEdit) {
                var languageNameToTranslate = '';
                $scope.supportedLanguages.forEach(function(x) {
                    if (x.Code === language.LanguageShortName) {
                        languageNameToTranslate = x.Language;
                        return;
                    }
                });
                var languageNameModel = {
                    CurrentLanguageCode: "en",
                    TargetLanguageCode: language.LanguageShortName,
                    ContentForTranslation : [languageNameToTranslate]
                }
                translateService.PostLanguageTranslation(languageNameModel).then(function(x) {
                    if (x.IsValid) {
                        if (languageEdit) {
                            $scope.languageForEdit.LanguageName = x.Data.Result.text[0];
                        } else {
                            $scope.newLanguage.LanguageName = x.Data.Result.text[0];
                        }
                    } 
                });
            }
            // ********************
            function validateErrorMessage(header, x) {
                if ($scope.translation[x.ErrorMessage] === undefined)
                    return $scope.translation[header] + ' ' + x.ErrorMessage;
                else
                    return $scope.translation[header] + ' ' + $scope.translation[x.ErrorMessage];
            }

            function initContent() {
                $scope.getAllLanguages();
                $scope.getTranslateServiceName();
                $scope.getSupportedLanguages();

            };
            //base init
            initContent();
        }]);
})();

