(function () {
    angular
        .module('webBellwether')
        .controller('editGameFeaturesController', ['$scope', '$timeout','translateService', 'integrationGamesService', function ($scope, $timeout,translateService, integrationGamesService) {
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
                integrationGamesService.PutGameFeature(gameFeatureModel).then(function () {
                    $scope.resultStateGameFeature = gameFeature.id;
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.GameFeatureEdited,
                        type: 'success'        
                    });
                },
                function () {
                    $scope.resultStateGameFeature = gameFeature.id + 999;
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: $scope.translation.GameFeatueNotEdited,
                        type: 'warning'
                    });
                });
            };

            $scope.editGameFeatureDetail = function (gameFeatureDetail) {
                integrationGamesService.PutGameFeatureDetail(gameFeatureDetail).then(function () {
                    $scope.resultStateGameFeatureDetail = gameFeatureDetail.id;
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.GameFeatureDetailEdited,
                        type: 'success'
                    });

                },
                function () {
                    $scope.resultStateGameFeatureDetail = gameFeatureDetail.id + 999;
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: $scope.translation.GameFeatureDetailNotEdited,
                        type: 'warning'
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
                    $scope.gameFeatures = x.data;
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.GameFeaturesAdded,
                        type: 'success'
                    });
                }, function() {
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: $scope.translation.GameFeaturesNotAdded,
                        type: 'warning'
                    });
                });
            };

            //translation
            $scope.translateGameFeatures = function(targetLanguage,gameFeatures) {
                var translateLanguageModel = {
                    CurrentLanguageCode: $scope.getTemplateLanguageShortName(),
                    TargetLanguageCode: targetLanguage.languageShortName,
                    ContentForTranslation: []
                };
                gameFeatures.forEach(function(x) {
                    translateLanguageModel.ContentForTranslation.push(x.gameFeatureTemplateName);
                });
                translateService.PostLanguageTranslation(translateLanguageModel).then(function (x) {
                    for (var i = 0; i < gameFeatures.length; i++) {
                        $scope.gameFeatures[i].gameFeatureName = x.data.text[i];
                    }
                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.TranslationCompletedSuccessfully,
                        type: 'success'
                    });
                }, function (x) {
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content:$scope.translation.ErrorDuringTranslation + ' ' + x.data.message,
                        type: 'alert',
                        timeout: 10000
                    });
                });

            };
            $scope.translateGameFeaturesDetails = function (targetLanguage,gameFeaturesDetails) {
                var translateLanguageModel = {
                    CurrentLanguageCode: $scope.getTemplateLanguageShortName(),
                    TargetLanguageCode: targetLanguage.languageShortName,
                    ContentForTranslation: []
                };
                gameFeaturesDetails.forEach(function(x) {
                    translateLanguageModel.ContentForTranslation.push(x.gameFeatureDetailTemplateName);
                });
                translateService.PostLanguageTranslation(translateLanguageModel).then(function (x) {
                    for (var i = 0; i < gameFeaturesDetails.length; i++) {
                        $scope.gameFeatureDetails[i].gameFeatureDetailName = x.data.text[i];
                    }

                    $.Notify({
                        caption: $scope.translation.Success,
                        content: $scope.translation.TranslationCompletedSuccessfully,
                        type: 'success'
                    });
                }, function (x) {
                    $.Notify({
                        caption: $scope.translation.Failure,
                        content: $scope.translation.ErrorDuringTranslation + ' ' + x.data.message,
                        type: 'alert',
                        timeout: 10000
                    });
                });

            };
            $scope.getTemplateLanguageShortName = function() {
                var templateLanguageShortName = '';
                $scope.languages.forEach(function(x) {
                    if (x.languageName === "English")
                        templateLanguageShortName = x.languageShortName;
                });
                return templateLanguageShortName;
            };

            //get on load , but i must change this value when i want add game in another language
            $scope.getGameFeatureDetails($scope.selectedLanguage);   
        }]);
})();
