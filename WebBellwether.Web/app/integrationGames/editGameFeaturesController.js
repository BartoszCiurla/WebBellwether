(function () {
    angular
        .module('webBellwether')
        .controller('editGameFeaturesController', ['$scope', '$timeout', 'integrationGamesService', function ($scope, $timeout, integrationGamesService) {
            $scope.resultStateGameFeature = '';
            $scope.resultStateGameFeatureDetail = '';
            $scope.gameFeatureDetails = [];
            $scope.getGameFeatureDetails = function (lang) {
                integrationGamesService.GameFeatureDetails(lang).then(function (results) {
                    $scope.gameFeatureDetails = results.data;
                    console.log("fill game feature details");
                });
            };

            $scope.getClass = function (id, id2) {
                //for gameFeature
                if (id > 0) {
                    if (id === $scope.resultStateGameFeature)
                        return 'success';
                    if (id + 999 === $scope.resultStateGameFeature)
                        return 'error';
                }
                //for gameFeatureDetail
                if (id2 > 0) {
                    if (id2 === $scope.resultStateGameFeatureDetail)
                        return 'success';
                    if (id2 + 999 === $scope.resultStateGameFeatureDetail)
                        return 'error';
                }
                return '';
            };

            $scope.editGameFeature = function (gameFeature) {
                var gameFeatureModel = {
                    Id: gameFeature.id,
                    GameFeatureName: gameFeature.gameFeatureName,
                    LanguageId: gameFeature.languageId
                };
                integrationGamesService.PutGameFeature(gameFeatureModel).then(function (response) {
                    console.log(response);
                    $scope.resultStateGameFeature = gameFeature.id;
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.GameFeatureEdited,
                        type: 'success',          
                    });
                },
                function (response) {
                    console.log(response);
                    $scope.resultStateGameFeature = gameFeature.id + 999;
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: $scope.translation.GameFeatueNotEdited,
                        type: 'warning',
                    });
                });
            };

            $scope.editGameFeatureDetail = function (gameFeatureDetail) {
                integrationGamesService.PutGameFeatureDetail(gameFeatureDetail).then(function (response) {
                    console.log(response);
                    $scope.resultStateGameFeatureDetail = gameFeatureDetail.id;
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.GameFeatureDetailEdited,
                        type: 'success',
                    });

                },
                function (response) {
                    console.log(response);
                    $scope.resultStateGameFeatureDetail = gameFeatureDetail.id + 999;
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: $scope.translation.GameFeatureDetailNotEdited,
                        type: 'warning',
                    });
                });
            };


            $scope.getGameFeatures = function (lang) {
                $scope.getGameFeatureDetails(lang);
                integrationGamesService.GameFeatures(lang).then(function (results) {
                    $scope.gameFeatures = results.data;
                    console.log("get game features");
                });
            };

            $scope.createGameFeatures = function(languageId) {
                integrationGamesService.PostCreateGameFeatures(languageId).then(function(x) {

                }, function(x) {

                });
            };
            //get on load , but i must change this value when i want add game in another language
            $scope.getGameFeatureDetails($scope.selectedLanguage);   
        }]);
})();
