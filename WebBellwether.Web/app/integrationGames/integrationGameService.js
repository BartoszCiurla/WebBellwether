(function () {
    angular
        .module('webBellwether')
        .factory('integrationGameService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;

            var postIntegrationGame = function (gameModel) {
                return $http.post(serviceBase + 'api/IntegrationGameManagement/PostIntegrationGame', gameModel).then(function (x) {
                    return x.data;
                });
            };
            var postCreateGameFeatures = function(languageid) {
                return $http.post(serviceBase + 'api/GameFeatureManagement/PostCreateGameFeatures/?languageId=' + languageid).then(function (x) {
                    return x.data;
                });
            }

            var putGameFeature = function (gameFeature) {
                return $http.post(serviceBase + 'api/GameFeatureManagement/PostGameFeature', gameFeature).then(function (x) {
                    return x.data;
                });
            };
            var putGameFeatures = function(gameFeatures) {
                return $http.post(serviceBase + 'api/GameFeatureManagement/PostGameFeatures', gameFeatures).then(function(x) {
                    return x.data;
                });
            };

            var putGameFeatureDetail = function (gameFeatureDetail) {
                return $http.post(serviceBase + 'api/GameFeatureManagement/PostGameFeatureDetail/', gameFeatureDetail).then(function (x) {
                    return x.data;
                });
            };

            var putGameFeatureDetails = function(gameFeatureDetails) {
                return $http.post(serviceBase + 'api/GameFeatureManagement/PostGameFeatureDetails/', gameFeatureDetails).then(function(x) {
                    return x.data;
                });
            };

            var deleteIntegrationGame = function (integrationGame) {
                return $http.post(serviceBase + 'api/IntegrationGameManagement/PostDeleteIntegrationGame/', integrationGame).then(function (x) {
                    return x.data;
                });
            };

            var putIntegrationGame = function (integrationGame) {
                return $http.post(serviceBase + 'api/IntegrationGameManagement/PostEditIntegrationGame/', integrationGame).then(function (x) {
                    return x.data;
                });
            };

            var getIntegrationGamesWithAvailableLanguages = function(languageid) {
                return $http.get(serviceBase + 'api/IntegrationGameManagement/GetIntegrationGamesWithAvailableLanguage/?languageId=' + languageid).then(function (x) {
                    return x.data;
                });
            };
            var getIntegrationGames = function (languageid) {
                return $http.get(serviceBase + 'api/IntegrationGame/GetIntegrationGames/?languageId=' + languageid).then(function (x) {
                    return x.data;
                });
            };

            var getGameFeatureDetails = function (languageid) {
                return $http.get(serviceBase + 'api/GameFeatureManagement/GetGameFeatureDetails/?languageId=' + languageid).then(function (x) {
                    return x.data;
                });
            };

            var getGameFeatures = function (languageid) {
                return $http.get(serviceBase + 'api/GameFeatureManagement/GetGameFeatures/?languageId=' + languageid).then(function (x) {
                    return x.data;
                });
            };
            var getGameFeatuesModelWithDetails = function (languageid) {
                return $http.get(serviceBase + 'api/GameFeatureManagement/GetGameFeatuesModelWithDetails/?languageId=' + languageid).then(function (x) {
                    return x.data;
                });
            };
            var getIntegrationGameTranslation = function (id, language) {                
                return $http.get(serviceBase + 'api/IntegrationGameManagement/GetIntegrationGameTranslation', { params: { gameId: id, languageId: language } }).then(function (x) {
                    return x.data;
                });
            };
            var integrationGamesServiceFactory = {};
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
            integrationGamesServiceFactory.PostCreateGameFeatures = postCreateGameFeatures;
            integrationGamesServiceFactory.PutGameFeatures = putGameFeatures;
            integrationGamesServiceFactory.PutGameFeatureDetails = putGameFeatureDetails;

            return integrationGamesServiceFactory;

        }]);
})();
