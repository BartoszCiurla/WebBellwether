(function () {
    angular
        .module('webBellwether')
        .controller('managementLanguagesController', ['$scope', '$timeout', 'languagesService', 'translateService', 'sharedService', function ($scope, $timeout, languagesService, translateService, sharedService) {
            var userNotification = '';
            $scope.currentPage = 0;
            $scope.pageSize = 20;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.selectedLanguageContent.length / $scope.pageSize);
            }
            $scope.goToTop = function () {
                window.scrollTo(0, 990);
            };
            $scope.activeLanguageKey = '';
            $scope.setActiveLanguageKey = function (active) {
                if (active.key != $scope.activeLanguageKey)
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
                    $scope.newLanguageOption = true

                }
                else {
                    $scope.newLanguageOption = false;
                }
            }
            //***********************

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
                        type: 'success',
                    });
                }, function (x) {
                    $scope.userNotification = $scope.translation.LanguageNotAdded;
                    if (x.data.message == "CriticalError")
                        $scope.userNotification += ' ' + $scope.translation.CriticalError;
                    else if (x.data.message == "LanguageExists")
                        $scope.userNotification += ' ' + $scope.translation.LanguageExists;
                    else if (x.data.message == "LanguageNotExists")
                        $scope.userNotification += ' ' + $scope.translation.LanguageNotExists;
                    else if (x.data.message == "LanguageFileNotExists")
                        $scope.userNotification += ' ' + $scope.translation.LanguageFileNotExists;
                    else
                        $scope.userNotification += ' ' + x.data.message;

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
                        type: 'success',
                    });

                }, function (x) {
                    $scope.userNotification = $scope.translation.LanguageKeyValueNotEdited;
                    if (x.data.message == "CriticalError")
                        $scope.userNotification += ' ' + $scope.translation.CriticalError;
                    else
                        $scope.userNotification += ' ' + x.data.message;
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
                        content: $scope.translation.LanguageKeyValueEdited,
                        type: 'success',
                    });
                }, function (x) {
                    $scope.userNotification = $scope.translation.LanguageNotEdited;
                    if (x.data.message == "CriticalError")
                        $scope.userNotification += ' ' + $scope.translation.CriticalError;
                    else
                        $scope.userNotification += ' ' + x.data.message;
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: $scope.userNotification,
                        type: 'alert',
                        timeout: 10000
                    });
                });
            };
            //***********************

            //publish language
            $scope.publishLanguage = function (language) {
                var langExistsInCurrentList = false;
                languagesService.PutPublishLanguage(language).then(function (x) {

                    $scope.languages.forEach(function (x) {
                        if (x.id == language.id)
                            langExistsInCurrentList = true;
                    });

                    $scope.userNotification = $scope.translation.LanguageEdited;

                    if (x.data == "LanguageHasBeenPublished") {
                        $scope.userNotification += ' ' + $scope.translation.LanguageHasBeenPublished;

                        if (!langExistsInCurrentList)
                            $scope.languages.push(language);
                    }
                    else {
                        $scope.userNotification += ' ' + $scope.translation.LanguageHasBeenNonPublic;
                        if (langExistsInCurrentList)
                            //public languages
                            angular.forEach($scope.languages, function (z, index) {
                                if (z.id == language.id)
                                    $scope.languages.splice(index, 1);
                            });
                    }
                    //management languages
                    angular.forEach($scope.allLanguages, function (z) {
                        if (z.id == language.id)
                            z.isPublic = language.isPublic;
                    })

                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.userNotification,
                        type: 'success',
                    });

                }, function (x) {
                    $scope.userNotification = $scope.translation.LanguageNotEdited;
                    if (x.data.message == "LanguageFileNotExists")
                        $scope.userNotification += ' ' + $scope.translation.LanguageFileNotExists;
                    else if (x.data.message == "EmptyKeysExists")
                        $scope.userNotification += ' ' + $scope.translation.EmptyKeyExists;
                    else if (x.data.message == "OnlyOnePublicLanguage")
                        $scope.userNotification += ' ' + $scope.translation.OnlyOnePublicLangugae;
                    else if (x.data.message == "LanguageNotExists")
                        $scope.userNotification += ' ' + $scope.translation.LanguageNotExists;
                    else
                        $scope.userNotification += ' ' + $scope.translation.CriticalError;

                    language.isPublic = !language.isPublic;

                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: $scope.userNotification,
                        type: 'alert',
                    });
                })
            };

            //***********************

            //language content
            $scope.selectedLanguageForEdit = null;
            $scope.selectedLanguageContent = [];
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
                    languagesService.GetLanguageContent(lang.id).then(function (x) {
                        for (key in x) {
                            //bad solution but the only thing that works
                            if (key.indexOf('$', 0) == -1 && key != "toJSON")
                                $scope.selectedLanguageContent.push({ key: key, value: x[key] });
                        }
                        $scope.changeSearchSource();
                    })
                }
            };
            // ********************

            //languages list
            $scope.allLanguages = [];
            $scope.getAllLanguages = function () {
                languagesService.GetAllLanguages().then(function (x) {
                    $scope.allLanguages = x.data;
                    $scope.allLanguages.forEach(function (z) {
                        if (z.id == $scope.selectedLanguage)
                            $scope.getLanguageContent(z);
                    })
                });
            };
            // ********************

            function initContent() {
                $scope.getAllLanguages();
            };
            //base init
            initContent();
        }]);
})();
