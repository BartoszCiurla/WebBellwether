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
                window.scrollTo(0, 470);
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
                        });
                    });
                });
            };
            //***********************

            //language content
            $scope.selectedLanguageForEdit = null;
            $scope.selectedLanguageContent = [];
            $scope.getLanguageContent = function (lang) {
                $scope.selectedLanguageContent = [];
                if (lang != null) {
                    $scope.languageForEdit = {
                        id: lang.id,
                        languageFlag: lang.languageFlag,
                        languageName: lang.languageName,
                        languageShortName: lang.languageShortName
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

        }]);
})();
