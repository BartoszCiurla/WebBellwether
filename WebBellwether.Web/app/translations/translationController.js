(function () {
    angular
        .module('webBellwether')
        .controller("translationController", function ($scope,$location,sharedService, translationService,localStorageService) {
            $scope.translate = function (select) {
                translationService.getTranslation($scope, select);
                $scope.selectedLanguage = select;
                localStorageService.set('mylanguage', select);
                sharedService.languageChange(select);
                //$location.path('/home');
            };
            $scope.languages = [];
            translationService.getLanguages().then(function (results) {
                $scope.languages = results.data;
            }), function (error) {
                console.log("error in get languages" + error);
            };
            $scope.selectedLanguage = (Number)(localStorageService.get('mylanguage') != null ? localStorageService.get('mylanguage') : 1);
        
            $scope.translate($scope.selectedLanguage);
    });
})();
