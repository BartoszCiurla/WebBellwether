(function () {
    angular
        .module('webBellwether')
        .controller('managementLanguagesController', ['$scope', '$timeout', 'languagesService', 'translateService', 'sharedService', function ($scope, $timeout, languagesService, translateService, sharedService) {
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
                if (active.key !== $scope.activeLanguageKey)
                    $scope.activeLanguageKey = active.key;
                else {
                    $scope.activeLanguageKey = null;
                }
            };

            //search 
            $scope.searchSource = [];
            $scope.changeSearchSource = function () {
                $scope.searchSource = [];
                $scope.selectedLanguageContent.forEach(function (z) {
                    $scope.searchSource.push(z.key);
                    $scope.searchSource.push(z.value);
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
                languagesService.PostLanguage(language).then(function (x) {
                    //insert new language to edit list 
                    $scope.allLanguages.push(x.data);
                    //feel key,value list with new lang 
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.LanguageAdded,
                        type: 'success'
                    });
                }, function (x) {
                    userNotification = $scope.translation.LanguageNotAdded + ' ' + $scope.translation[x.data.message];
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                        timeout: 10000
                    });
                });
            };
            //***********************

            //edit language key
            $scope.languageForEdit = null;
            $scope.editLanguageKey = function (selectedLanguageKey, lang) {
                var languageKey = {
                    Key: selectedLanguageKey.key,
                    Value: selectedLanguageKey.value,
                    LanguageId: lang.id
                };
                languagesService.PutLanguageKey(languageKey).then(function (x) {
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.LanguageKeyValueEdited,
                        type: 'success'
                    });

                }, function (x) {
                    userNotification = $scope.translation.LanguageKeyValueNotEdited + ' ' + $scope.translation[x.data.message];
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                        timeout: 10000
                    });
                });
            };
            //***********************

            //edit language
            $scope.editLanguage = function (language) {
                languagesService.PutLanguage(language).then(function (x) {
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.LanguageEdited,
                        type: 'success'
                    });
                }, function (x) {
                    userNotification = $scope.translation.LanguageNotEdited + ' ' + $scope.translation[x.data.message];
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                        timeout: 10000
                    });
                });
            };
            //***********************


            //delete language
            $scope.deleteDialog = function () {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.open();
            };
            //version alpha
            $scope.deleteLanguageFromAllList = function (language) {
                angular.forEach($scope.languages, function (z, index) {
                    if (z.id === language.id)
                        $scope.languages.splice(index, 1);
                });

                angular.forEach($scope.allLanguages, function (z, index) {
                    if (z.id === language.id)
                        $scope.allLanguages.splice(index, 1);
                });
                $scope.languageForEdit = null;
            };
            $scope.deleteLanguage = function (language) {
                var dialog = $('#deleteDialog').data('dialog');
                dialog.close();
                console.log(language);
                languagesService.DeleteLanguage(language).then(function (x) {
                    $scope.deleteLanguageFromAllList(language);

                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.LanguageRemoved,
                        type: 'success'
                    });
                }, function (x) {
                    userNotification = $scope.translation.LanguageNotRemoved + ' ' + $scope.translation[x.data.message];
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                        timeout: 10000
                    });
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
                    if (z.id === language.id)
                        $scope.languages.splice(index, 1);
                });
            };
            $scope.checkIsLangExistsInGlobalList = function (language) {
                var langExistsInCurrentList = false;
                $scope.languages.forEach(function (x) {
                    if (x.id === language.id)
                        langExistsInCurrentList = true;
                });
                return langExistsInCurrentList;
            };
            $scope.publishLanguage = function (language) {
                var langExistsInCurrentList = $scope.checkIsLangExistsInGlobalList(language);

                languagesService.PutPublishLanguage(language).then(function (x) {
                    userNotification = $scope.translation.LanguageEdited;

                    if (x.data === "LanguageHasBeenPublished") {
                        $scope.languageHasBeenPublished(language, langExistsInCurrentList);

                    } else if (x.data === "LanguageHasBeenNonpublic") {
                        $scope.languageHasBeenUnPublic(language);
                    }
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: userNotification,
                        type: 'success'
                    });

                }, function (x) {
                    userNotification = $scope.translation.LanguageNotEdited + ' ' + $scope.translation[x.data.message];

                    language.isPublic = !language.isPublic;

                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert'
                    });
                });
            };

            //***********************

            //language content
            $scope.selectedLanguageForEdit = null;
            $scope.selectedLanguageContent = [];
            $scope.getLanguageFileContent = function(lang) {
                languagesService.GetLanguageContent(lang.id).then(function (x) {
                    $scope.selectedLanguageContent = [];
                    for (key in x) {
                        if (x.hasOwnProperty(key)) {
                            if (key.indexOf('$', 0) === -1 && key !== "toJSON")                                
                                $scope.selectedLanguageContent.push({ key: key, value: x[key] });
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
                        id: lang.id,
                        languageFlag: lang.languageFlag,
                        languageName: lang.languageName,
                        languageShortName: lang.languageShortName,
                        isPublic: lang.isPublic
                    };
                    $scope.getLanguageFileContent(lang);
                }
            };
            // ********************

            //languages list
            $scope.allLanguages = [];
            $scope.getAllLanguages = function () {
                languagesService.GetAllLanguages().then(function (x) {
                    $scope.allLanguages = x.data;
                    $scope.allLanguages.forEach(function (z) {
                        if (z.id === $scope.selectedLanguage)
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
                    $scope.supportedLanguages = x.data;
                });
            };
            $scope.getTranslateServiceName = function () {
                translateService.GetTranslateServiceName().then(function (x) {
                    $scope.translateServiceName = x.data;
                });
            };
            // ********************

            //translate function
            $scope.translateLanguageKey = function (currentLangId, targetLang, content, key) {
                var shortNameCurrentLang = '';
                $scope.languages.forEach((function (x) {
                    if (x.id === currentLangId)
                        shortNameCurrentLang = x.languageShortName;
                }));
                var translateLanguageKeyModel = {
                    CurrentLanguageShortName: shortNameCurrentLang,
                    TargetLangaugeShortName: targetLang,
                    ContentToTranslate: content
                };
                translateService.PostLanguageKeyTranslation(translateLanguageKeyModel).then(function (x) {
                    $scope.selectedLanguageContent.forEach(function (z) {
                        if (z.key === key) {
                            z.value = x.data.text[0];
                        }
                    });
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.TranslationCompletedSuccessfully + " " + x.data.text[0],
                        type: 'success',
                        timeout: 10000
                    });

                }, function (x) {
                    userNotification = $scope.translation.ErrorDuringTranslation + ' ' + x.data.message;
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                        timeout: 10000
                    });
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
                    if (x.id === currentLangId)
                        selectedLangauge = x;
                }));
                var translateLangaugeKeysModel = {
                    CurrentLanguageShortName: selectedLangauge.languageShortName,
                    CurrentLanguageId: selectedLangauge.id,
                    TargetLangaugeShortName: targetLang.languageShortName,
                    TargetLanguageId: targetLang.id
                };
                translateService.PostTranslateAllLanguageKeys(translateLangaugeKeysModel).then(function (x) {
                    $scope.getLanguageFileContent(targetLang);
                    $scope.translateAllLangaugeKeysCloseDialog();
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.KeyValuesAreCorrectlyFilled,
                        type: 'success',
                        timeout: 10000
                    });
                }, function (x) {
                    $scope.translateAllLangaugeKeysCloseDialog();
                    userNotification = $scope.translation.KeyValuesAreNotFilled;
                    var haveResultMessage = $scope.translation[x.data.message];
                    if (haveResultMessage != undefined) {
                        userNotification += ' ' + haveResultMessage;
                    } else {
                        userNotification += ' ' + x.data.message;
                    }
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: userNotification,
                        type: 'alert',
                        timeout: 10000
                    });
                });
            };
            // ********************

            function initContent() {
                $scope.getAllLanguages();
                $scope.getTranslateServiceName();
                $scope.getSupportedLanguages();

            };
            //base init
            initContent();
        }]);
})();

