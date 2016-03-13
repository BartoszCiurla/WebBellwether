(function () {
    angular
        .module('webBellwether')
        .controller('jokeController', ['$scope', 'sharedService', 'jokeService', 'startFromFilter', function ($scope, sharedService, jokeService, startFromFilter) {
            //pagination
            $scope.search = {};
            $scope.resetFilters = function () {
                $scope.search = {};
            };
            $scope.currentPage = 1;
            $scope.entryLimit = 6;
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
                jokeService.GetJokes(languageId).then(function (x) {
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