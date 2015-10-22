(function () {
    angular
        .module('webBellwether')
        .factory('jokesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var postJokeCategory = function (jokeCategory) {
                return $http.post(serviceBase + 'api/Jokes/PostJokeCategory', jokeCategory).then(function (x) {
                    return x;
                });
            };
            var getJokeCategoriesWithAvailableLanguage = function (languageid) {
                return $http.get(serviceBase + 'api/Jokes/GetJokeCategoriesWithAvailableLanguage/?language=' + languageid).then(function (x) {
                    return x;
                });
            };

            var getJokeCategoryTranslation = function (id, language) {
                return $http.get(serviceBase + 'api/Jokes/GetJokeCategoryTranslation', { params: { id: id, languageId: language } }).then(function (x) {
                    return x;
                });
            };

            var putJokeCategory = function (jokeCategory) {
                return $http.post(serviceBase + 'api/Jokes/PostEditJokeCategory/', jokeCategory).then(function (x) {
                    return x;
                });
            };
            var deleteJokeCategory = function (jokeCategory) {
                return $http.post(serviceBase + 'api/Jokes/PostDeleteJokeCategory/', jokeCategory).then(function (x) {
                    return x;
                });
            }

            var jokesServiceFactory = {};
            jokesServiceFactory.PostJokeCategory = postJokeCategory;
            jokesServiceFactory.GetJokeCategoriesWithAvailableLanguage = getJokeCategoriesWithAvailableLanguage;
            jokesServiceFactory.GetJokeCategoryTranslation = getJokeCategoryTranslation;
            jokesServiceFactory.PutJokeCategory = putJokeCategory;
            jokesServiceFactory.DeleteJokeCategory = deleteJokeCategory;
            return jokesServiceFactory;

        }]);
})();
