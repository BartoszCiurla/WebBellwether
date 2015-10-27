(function () {
    angular
        .module('webBellwether')
        .controller('managementLanguagesController', ['$scope', '$timeout', 'languagesService', 'translateService', 'sharedService', function ($scope, $timeout, languagesService, translateService, sharedService) {

            //only test 
            $scope.languageForEdit = '';
            $scope.myContent = '';
            $scope.getContent = function (lang) {
                
                $scope.myContent = languagesService.GetContent(lang);
               
            };

            $scope.getContent("2");

        }]);
})();
