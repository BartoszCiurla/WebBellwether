(function () {
    angular
        .module('webBellwether')
        .factory('jokesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var postJokeCategory = function (jokeCategory) {
                return $http.post(serviceBase = 'api/Jokes/PostJokeCategory', jokeCategory).then(function (x) {
                    return x;
                });
            };
            var getJokeCategoriesWithAvailableLanguage = function (languageid) {
                return $http.get(serviceBase + 'api/Jokes/GetJokeCategoriesWithAvailableLanguage/?language=' + languageid).then(function (x) {
                    return x;
                });
            };

            var jokesServiceFactory = {};
            jokesServiceFactory.PostJokeCategory = postJokeCategory;
            jokesServiceFactory.GetJokeCategoriesWithAvailableLanguage = getJokeCategoriesWithAvailableLanguage;
            return jokesServiceFactory;

        }]);
})();
