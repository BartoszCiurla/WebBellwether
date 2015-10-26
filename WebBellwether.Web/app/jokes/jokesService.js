(function () {
    angular
        .module('webBellwether')
        .factory('jokesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var postJoke = function (joke) {
                return $http.post(serviceBase + 'api/Jokes/PostJoke', joke).then(function (x) {
                    return x;
                });
            };
            var postJokeCategory = function (jokeCategory) {
                return $http.post(serviceBase + 'api/Jokes/PostJokeCategory', jokeCategory).then(function (x) {
                    return x;
                });
            };
            var deleteJoke = function (joke) {
                return $http.post(serviceBase + 'api/Jokes/PostDeleteJoke', joke).then(function (x) {
                    return x;
                });
            };
            var putJoke = function (joke) {
                return $http.post(serviceBase + 'api/Jokes/PostEditJoke', joke).then(function (x) {
                    return x;
                });
            };
            var getJokeTranslation = function (id, language) {
                return $http.get(serviceBase + 'api/Jokes/GetJokeTranslation', { params: { id: id, languageId: language } }).then(function (x) {
                    return x;
                });
            };
            var getJokeCategoriesWithAvailableLanguage = function (languageid) {
                return $http.get(serviceBase + 'api/Jokes/GetJokeCategoriesWithAvailableLanguage/?language=' + languageid).then(function (x) {
                    return x;
                });
            };
            var getJokeWithAvailableLanguages = function (languageid) {
                return $http.get(serviceBase + 'api/Jokes/GetJokesWithAvailableLanguages/?language=' + languageid).then(function (x) {
                    return x;
                });
            };
            var getJokeCategories = function (languageId) {
                return $http.get(serviceBase + 'api/Jokes/GetJokeCategories/?language=' + languageId).then(function (x) {
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
            jokesServiceFactory.GetJokeCategories = getJokeCategories;
            jokesServiceFactory.PostJoke = postJoke;
            jokesServiceFactory.GetJokeWithAvailableLanguages = getJokeWithAvailableLanguages;
            jokesServiceFactory.DeleteJoke = deleteJoke;
            jokesServiceFactory.PutJoke = putJoke;
            jokesServiceFactory.GetJokeTranslation = getJokeTranslation;
            return jokesServiceFactory;

        }]);
})();
