(function () {
    angular
        .module('webBellwether')
        .factory('integrationGamesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var integrationGamesServiceFactory = {};

            var postIntegrationGame = function (gameModel) {
                return $http.post(serviceBase + 'api/IntegrationGames/PostIntegrationGame', gameModel).then(function (x) {
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

            };
            var deleteIntegrationGame = function (integrationGame) {
                return $http.post(serviceBase + 'api/IntegrationGames/PostDeleteIntegrationGame/', integrationGame).then(function (x) {
                    return x;
                })
            };

            var putIntegrationGame = function (integrationGame) {
                return $http.post(serviceBase + 'api/IntegrationGames/PostEditIntegrationGame/', integrationGame).then(function (x) {
                    return x;
                });
            };

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
            };
            var getGameFeatuesModelWithDetails = function (languageid) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetGameFeatuesModelWithDetails/?language=' + languageid).then(function (x) {
                    return x;
                });
            };
            var getIntegrationGameTranslation = function (id,language) {
                return $http.get(serviceBase + 'api/IntegrationGames/GetIntegrationGameTranslation', { params: { id: id, languageId :language} }).then(function (x) {
                    return x;
                });
            };

            integrationGamesServiceFactory.GameFeatuesModelWithDetails = getGameFeatuesModelWithDetails;
            integrationGamesServiceFactory.AddNewIntegrationGame = postIntegrationGame;
            integrationGamesServiceFactory.IntegrationGames = getIntegrationGames;
            integrationGamesServiceFactory.IntegrationGamesWithAvailableLanguages = getIntegrationGamesWithAvailableLanguages;
            integrationGamesServiceFactory.GameFeatureDetails = getGameFeatureDetails;
            integrationGamesServiceFactory.GameFeatures = getGameFeatures;
            integrationGamesServiceFactory.PutIntegrationGame = putIntegrationGame;
            integrationGamesServiceFactory.PutGameFeature = putGameFeature;
            integrationGamesServiceFactory.PutGameFeatureDetail = putGameFeatureDetail;
            integrationGamesServiceFactory.DeleteIntegrationGame = deleteIntegrationGame;
            integrationGamesServiceFactory.GetIntegrationGameTranslation = getIntegrationGameTranslation;

            return integrationGamesServiceFactory;

        }]);
})();
