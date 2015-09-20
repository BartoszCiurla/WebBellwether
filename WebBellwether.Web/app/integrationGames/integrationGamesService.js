(function () {
    angular
        .module('webBellwether')
        .factory('integrationGamesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var integrationGamesServiceFactory = {};

            var postIntegrationGame = function (gameModel) {
                return $http.post(serviceBase + 'api/IntegrationGames/PostIntegrationGame', JSON.stringify(gameModel)).then(function (x) {
                    return x;
                });
            };

            var putGameFeature = function (gameFeature) {
                return $http.post(serviceBase + 'api/IntegrationGames/PostGameFeature', gameFeature).then(function (x) {
                    return x;
                });
            };

            var putGameFeatureDetail = function (gameFeatureDetail) {
                return $http.post(serviceBase + 'api/IntegrationGames/PostGameFeatureDetail/', gameFeatureDetail).then(function (x) {
                    return x;
                });

            }
            var getIntegrationGamesWithAvailableLanguages = function(languageid) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetIntegrationGamesWithAvailableLanguage/?language=' + languageid).then(function (x) {
                    return x;
                });
            };
            var getIntegrationGames = function (languageid) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetIntegrationGames/?language=' + languageid).then(function (x) {
                    return x;
                });
            };

            var getGameFeatureDetails = function (languageid) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetGameFeatureDetails/?language=' + languageid).then(function (x) {
                    return x;
                });
            };

            var getGameFeatures = function (languageid) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetGameFeatures/?language=' + languageid).then(function (x) {
                    return x;
                });
            }
            var getGameFeatuesModelWithDetails = function(languageid) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetGameFeatuesModelWithDetails/?language=' + languageid).then(function (x) {
                    return x;
                });
            }
            integrationGamesServiceFactory.GameFeatuesModelWithDetails = getGameFeatuesModelWithDetails;
            integrationGamesServiceFactory.AddNewIntegrationGame = postIntegrationGame;
            integrationGamesServiceFactory.IntegrationGames = getIntegrationGames;
            integrationGamesServiceFactory.IntegrationGamesWithAvailableLanguages = getIntegrationGamesWithAvailableLanguages;
            integrationGamesServiceFactory.GameFeatureDetails = getGameFeatureDetails;
            integrationGamesServiceFactory.GameFeatures = getGameFeatures;
            integrationGamesServiceFactory.PutGameFeature = putGameFeature;
            integrationGamesServiceFactory.PutGameFeatureDetail = putGameFeatureDetail;

            return integrationGamesServiceFactory;

        }]);
})();
