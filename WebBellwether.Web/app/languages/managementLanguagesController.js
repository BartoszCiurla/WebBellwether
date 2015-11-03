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

            //$scope.active = '';
            //$scope.showKeyDetail = function (languageKey) {
            //    if ($scope.active != languageKey.key)
            //        $scope.active = languageKey.key;
            //    else 
            //        $scope.active = null;
            //};

            $scope.languageForEdit = '';


            //edit language key
            $scope.editLanguageKey = function (selectedLanguageKey) {
                console.log(selectedLanguageKey);
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
