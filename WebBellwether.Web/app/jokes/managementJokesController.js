(function () {
    angular
        .module('webBellwether')
        .controller('managementJokesController', ['$scope','$timeout','sharedService','jokesService', function ($scope, $timeout, sharedService, jokesService) {
            var userNotification = '';
            //pagination and set other joke category translation (get from api)
            $scope.currentPage = 0;
            $scope.pageSize = 10;
            $scope.numberOfPages = function () {
                if ($scope.jokes.length > 0)
                    return Math.ceil($scope.jokes.length / $scope.pageSize);
                else
                    return 0;
            }
            //*******************

            //add new joke
            $scope.newJoke = '';
            $scope.addJoke = function (newjoke, language) {
                var joke = {
                    Id: '',
                    JokeId: '',
                    JokeContent: newjoke.jokeContent,
                    LanguageId: language,
                    JokeCategoryId :newjoke.jokeCategoryId
                };
                jokesService.PostJoke(joke).then(function (x) {
                    $scope.jokes.push(x.data);
                    $scope.newJoke = '';
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: 'BRAK TRANSLACJI DLA DODANIA ZARTU',
                        type: 'success',
                    });
                },
                function (x) {
                });
            };

            //*******************

            //jokes
            $scope.jokes = [];
            $scope.getJokeWithAvailableLanguages = function (lang) {
                jokesService.GetJokeWithAvailableLanguages(lang).then(function (x) {
                    $scope.jokes = [];
                    $scope.jokes = x.data;
                });
            };
            //*******************

            //joke categories
            $scope.jokeCategories = [];
            $scope.getJokeCategories = function (lang) {
                jokesService.GetJokeCategories(lang).then(function (x) {
                    $scope.jokeCategories = [];
                    $scope.jokeCategories = x.data;
                });
            };
            //*******************

            //init content
            function initContent(language) {
                $scope.getJokeCategories(language);
                $scope.getJokeWithAvailableLanguages(language);
            };
            //base init
            initContent($scope.selectedLanguage);

            //when language change
            $scope.$on('languageChange', function () {
                initContent(sharedService.sharedmessage);
            });
            //*******************
        }]);
})();