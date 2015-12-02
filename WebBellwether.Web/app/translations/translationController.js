(function () {
    angular
        .module('webBellwether')
        .controller("translationController", function ($scope, $location, sharedService, translationService, localStorageService) {
            $scope.languages = [];
            $scope.translate = function (select) {
                var isPublic = false;
                $scope.languages.forEach(function(x) {
                    if (x.id === select && x.isPublic)
                        isPublic = true;
                });
                if (!isPublic) {
                    $scope.languages.forEach(function(x) {
                        if (x.isPublic) {
                            select = x.id;
                            return;
                        }
                    });
                }
                //this is global value ... ffs 
                $scope.selectedLanguage = select;

                translationService.getTranslation($scope, select);
                localStorageService.set('mylanguage', select);
                sharedService.languageChange(select);
            };
            
            translationService.getLanguages().then(function (results) {
                $scope.languages = results.data;
                $scope.selectedLanguage = (Number)(localStorageService.get('mylanguage') != null ? localStorageService.get('mylanguage') : 1);
                //tutaj potrzebne zabezpieczenie jak jezyk nie jest publiczny to z automatu biore anga jak go nie ma to biore jakikolwiek
                $scope.translate($scope.selectedLanguage);

            }), function (error) {
                console.log("error in get languages" + error);
            };
           
    });
})();
