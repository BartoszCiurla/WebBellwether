(function () {
    angular
        .module('webBellwether')
        .factory('integrationGamesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var integrationGamesServiceFactory = {};

            var postIntegrationGame = function (gameModel) {
                return $http.post(serviceBase + 'api/IntegrationGames/PostIntegrationGame', gameModel).then(function (response) {
                    return response;
                });
            };

            var putGameFeature = function (gameFeature) {
                return $http.post(serviceBase + 'api/IntegrationGames/PostGameFeature', gameFeature).then(function (response) {
                    return response;
                });
            };

            var getIntegrationGames = function (languageid) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetIntegrationGames/?language=' + languageid).then(function (results) {
                    return results;
                });
            };

            var getGameDescription = function (languageid) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetGameDescription/?language=' + languageid).then(function (results) {
                    return results;
                });
            };

            var getGameFeatures = function (languageid) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetGameFeatures/?language=' + languageid).then(function (results) {
                    return results;
                });
            }

            integrationGamesServiceFactory.AddNewIntegrationGame = postIntegrationGame;
            integrationGamesServiceFactory.IntegrationGames = getIntegrationGames;
            integrationGamesServiceFactory.GameDescription = getGameDescription;
            integrationGamesServiceFactory.GameFeatures = getGameFeatures;
            integrationGamesServiceFactory.PutGameFeature = putGameFeature;

            return integrationGamesServiceFactory;

        }]);
})();
