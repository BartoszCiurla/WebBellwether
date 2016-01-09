(function () {
    angular
        .module('webBellwether')
        .factory('jokesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var getJokes = function(languageId) {
                return $http.get(serviceBase + 'api/Jokes/GetJokes/?languageId=' + languageId).then(function (x) {
                    return x.data;
                });
            };

            var postJoke = function (joke) {
                return $http.post(serviceBase + 'api/JokeManagement/PostJoke', joke).then(function (x) {
                    return x.data;
                });
            };
            var postJokeCategory = function (jokeCategory) {
                return $http.post(serviceBase + 'api/JokeCategoryManagement/PostJokeCategory', jokeCategory).then(function (x) {
                    return x.data;
                });
            };
            var deleteJoke = function (joke) {
                return $http.post(serviceBase + 'api/JokeManagement/PostDeleteJoke', joke).then(function (x) {
                    return x.data;
                });
            };
            var putJoke = function (joke) {
                return $http.post(serviceBase + 'api/JokeManagement/PostEditJoke', joke).then(function (x) {
                    return x.data;
                });
            };
            var getJokeTranslation = function (id, language) {
                return $http.get(serviceBase + 'api/JokeManagement/GetJokeTranslation', { params: { jokeId: id, languageId: language } }).then(function (x) {
                    return x.data;
                });
            };
            var getJokeCategoriesWithAvailableLanguage = function (languageid) {
                return $http.get(serviceBase + 'api/Jokes/GetJokeCategoriesWithAvailableLanguage/?languageId=' + languageid).then(function (x) {
                    return x.data;
                });
            };
            var getJokeWithAvailableLanguages = function (languageid) {
                return $http.get(serviceBase + 'api/Jokes/GetJokesWithAvailableLanguages/?languageId=' + languageid).then(function (x) {
                    return x.data;
                });
            };
            var getJokeCategories = function (languageId) {
                return $http.get(serviceBase + 'api/JokeCategoryManagement/GetJokeCategories/?languageId=' + languageId).then(function (x) {
                    return x.data;
                });
            };

            var getJokeCategoryTranslation = function (id, language) {
                return $http.get(serviceBase + 'api/JokeCategoryManagement/GetJokeCategoryTranslation', { params: { jokeCategoryId: id, languageId: language } }).then(function (x) {
                    return x.data;
                });
            };

            var putJokeCategory = function (jokeCategory) {
                return $http.post(serviceBase + 'api/JokeCategoryManagement/PostEditJokeCategory/', jokeCategory).then(function (x) {
                    return x.data;
                });
            };
            var deleteJokeCategory = function (jokeCategory) {
                return $http.post(serviceBase + 'api/JokeCategoryManagement/PostDeleteJokeCategory/', jokeCategory).then(function (x) {
                    return x.data;
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
            jokesServiceFactory.GetJokes = getJokes;
            return jokesServiceFactory;

        }]);
})();
