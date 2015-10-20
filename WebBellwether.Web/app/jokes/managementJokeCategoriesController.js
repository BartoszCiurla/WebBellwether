(function () {
    angular
        .module('webBellwether')
        .controller('managementJokeCategoriesController', ['$scope', '$timeout', 'sharedService', 'jokesService', function ($scope, $timeout, sharedService, jokesService) {
            var userNotification = '';
            //pagination and set other joke category translation (get from api)
            $scope.currentPage = 0;
            $scope.pageSize = 10;
            $scope.numberOfPages = function () {
                if ($scope.jokeCategories.length > 0)
                    return Math.ceil($scope.jokeCategories.length / $scope.pageSize);
                else
                    return 0;
            }
            $scope.selectedJokeCategory = '';
            $scope.selectedJokeCategoryTranslation = '';
            $scope.languageForOtherTranslation = '';
            //add new category
            $scope.addJokeCategory = function (newJokeCategory, selectedLanguage) {
                var jokeCategory = {
                    Id: '',
                    JokeCategoryId: '',
                    JokeCategoryName: newJokeCategory,
                    LanguageId: selectedLanguage,
                    JokeCategoryTranslations :[]
                }
                jokesService.PostJokeCategory(jokeCategory).then(function (x) {
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: 'categoria dodana',
                        type: 'success',
                    });
                },
                function (x) {
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: 'hmm categoria nie dodana',
                        type: 'alert',
                    });
                });
            };

            //joke categories
            $scope.jokeCategories = [];
            $scope.getJokeCategoriesWithAvailableLanguage = function (lang) {
                jokesService.GetJokeCategoriesWithAvailableLanguage(lang).then(function (x) {
                    $scope.jokeCategories = [];
                    $scope.jokeCategories = x.data;
                });
            };
            $scope.setJokeCategoryTranslationAfterLanguageChange = function () {
                if ($scope.selectedJokeCategory != '') {
                    $scope.jokeCategories.forEach(function (o) {
                        if (o.id == $scope.selectedJokeCategory.id && o.languageId == $scope.selectedLanguage) {
                            $scope.selectedJokeCategory = o;
                            return;
                        }
                    });
                    if($scope.selectedJokeCategory.languageId != $scope.selectedLanguage)
                        $scope.resetSelectedJokeCategoryOrTranslation(true, false);
                }
            };
            //reset selected game or translation true is reset 
            $scope.resetSelectedJokeCategoryOrTranslation = function (category, translation) {
                if (category == true)
                    $scope.selectedJokeCategory = '';
                if (translation == true)
                    $scope.selectedJokeCategoryTranslation = '';
            }
            // ********************

            //when language change 
            $scope.$on('languageChange', function () {
                $scope.getJokeCategoriesWithAvailableLanguage(sharedService.sharedmessage);
            });
            
            // ********************

            //base init
            $scope.getJokeCategoriesWithAvailableLanguage($scope.selectedLanguage);
            // ********************

        }]);
})();
