(function () {
    angular
        .module('webBellwether')
        .controller('managementLanguagesController', ['$scope', '$timeout', 'languagesService', 'translateService', 'sharedService', function ($scope, $timeout, languagesService, translateService, sharedService) {
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
                else{
                    $scope.activeLanguageKey = null;
                }
            };
            $scope.languageForEdit = '';
            //edit language key
            $scope.editLanguageKey = function (selectedLanguageKey,lang) {
                var languageKey = {
                    Key: selectedLanguageKey.key,
                    Value: selectedLanguageKey.value,
                    LanguageId: lang
                };
                languagesService.PutLanguageKey(languageKey).then(function (x) {

                }, function (x) {

                });
            };

            //language content
            $scope.selectedLanguageContent = [];
            $scope.getLanguageContent = function (lang) {
                $scope.selectedLanguageContent = [];
                languagesService.GetLanguageContent(lang).then(function (x) {
                    for (key in x) {
                        //bad solution but the only thing that works
                        if (key.indexOf('$', 0) == -1 && key != "toJSON")
                            $scope.selectedLanguageContent.push({ key: key, value: x[key] });
                    }
                })                
            };
            // ********************

        }]);
})();
