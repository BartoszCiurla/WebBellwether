(function () {
    angular
        .module('webBellwether')
        .controller('jokesController', ['$scope', 'sharedService', 'jokesService', 'startFromFilter', function ($scope, sharedService, jokesService, startFromFilter) {
            //pagination
            $scope.search = {};
            $scope.resetFilters = function () {
                $scope.search = {};
            };
            $scope.currentPage = 1;
            $scope.entryLimit = 12;
            $scope.noOfPages = 0;
            $scope.maxSize = 5; //max size in pager 


            $scope.updateSearch = function (jokesSearchParams) {
                $scope.search = jokesSearchParams;
                $scope.filtered = startFromFilter($scope.jokes, $scope.search);
            };
           
            $scope.goToTop = function () {
                window.scrollTo(0, 470);
            };
            $scope.pageChanged = function () {
                $scope.goToTop();
            };
            //**************
            $scope.getJokes = function (languageId) {
                jokesService.GetJokes(languageId).then(function (x) {
                    $scope.jokes = [];
                    $scope.jokes = x.Data;
                });
            };


            //base init
            function initContent(language) {
                if (language !== undefined) {
                    $scope.getJokes($scope.selectedLanguage);
                }
            };
            initContent($scope.selectedLanguage);
            // ********************

            $scope.$on("jokesSearchParamsChange", function () {
                $scope.updateSearch(sharedService.sharedmessage);
            });

            $scope.$on('languageChange', function () {
                $scope.getJokes(sharedService.sharedmessage);
            });
        }]);
})();