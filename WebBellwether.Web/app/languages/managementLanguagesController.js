(function () {
    angular
        .module('webBellwether')
        .controller('managementLanguagesController', ['$scope', '$timeout', 'languagesService', 'translateService', 'sharedService', function ($scope, $timeout, languagesService, translateService, sharedService) {
            $scope.currentPage = 0;
            $scope.pageSize = 20;
            //$scope.numberOfPages = function () {
            //    if (Object.keys($scope.selectedLanguageContent).length > 0)
            //        return Math.ceil(Object.keys($scope.selectedLanguageContent).length / $scope.pageSize);
            //    else
            //        return 0;
            //};
            //$scope.quotientKeyPageSize = function () {
            //    return (Object.keys($scope.selectedLanguageContent).length / $scope.pageSize - 1);
            //}
            $scope.numberOfPages = function () {
                return Math.ceil($scope.selectedLanguageContent.length / $scope.pageSize);
            }
            $scope.goToTop = function () {
                window.scrollTo(0, 470);
            };

            $scope.languageForEdit = '';

            $scope.selectedLanguageContent = [];
            $scope.getLanguageContent = function (lang) {
                //$scope.selectedLanguageContent = languagesService.GetLanguageContent(lang);
                $scope.selectedLanguageContent = [];
                languagesService.GetLanguageContent(lang).then(function (x) {
                    for (key in x) {
                        if(key.indexOf(1) != '$' && key != "toJSON")
                            $scope.selectedLanguageContent.push({ key: key, value: x[key] });
                    }
                })                
            };      
        }]);
})();
