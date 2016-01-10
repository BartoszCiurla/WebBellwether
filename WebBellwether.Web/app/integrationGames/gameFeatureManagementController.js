(function () {
    angular
        .module('webBellwether')
        .controller('gameFeatureManagementController', ['$scope', '$timeout', 'translateService', 'integrationGameService', function ($scope, $timeout, translateService, integrationGameService) {
            $scope.resultStateGameFeature = '';
            $scope.resultStateGameFeatureDetail = '';
            $scope.gameFeatureDetails = [];
            $scope.getGameFeatureDetails = function (lang) {
                if (lang !== undefined)
                    integrationGameService.GameFeatureDetails(lang).then(function (results) {
                        $scope.gameFeatureDetails = results.Data;
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
                    Id: gameFeature.Id,
                    GameFeatureName: gameFeature.GameFeatureName,
                    LanguageId: gameFeature.LanguageId
                };
                integrationGameService.PutGameFeature(gameFeatureModel).then(function (x) {
                    if (x.IsValid) {
                        $scope.resultStateGameFeature = gameFeature.Id;
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.GameFeatureEdited,
                            type: 'success'
                        });
                    } else {
                        $scope.resultStateGameFeature = gameFeature.Id + 999;
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: $scope.translation.GameFeatueNotEdited,
                            type: 'warning'
                        });
                    }
                });
            };

            $scope.editGameFeatureDetail = function (gameFeatureDetail) {
                integrationGameService.PutGameFeatureDetail(gameFeatureDetail).then(function (x) {
                    if (x.IsValid) {
                        $scope.resultStateGameFeatureDetail = gameFeatureDetail.Id;
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.GameFeatureDetailEdited,
                            type: 'success'
                        });
                    } else {
                        $scope.resultStateGameFeatureDetail = gameFeatureDetail.Id + 999;
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: $scope.translation.GameFeatureDetailNotEdited,
                            type: 'warning'
                        });
                    }
                });
            };

            $scope.getGameFeatures = function (lang) {
                if (lang !== undefined) {
                    $scope.getGameFeatureDetails(lang);
                    integrationGameService.GameFeatures(lang).then(function (results) {
                        $scope.gameFeatures = results.Data;
                    });
                }
            };

            $scope.createGameFeatures = function (languageId) {
                integrationGameService.PostCreateGameFeatures(languageId).then(function (x) {
                    if (x.IsValid) {
                        $scope.gameFeatures = x.Data;
                        $scope.getGameFeatureDetails(languageId);
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.GameFeaturesAdded,
                            type: 'success'
                        });
                    } else {
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: $scope.translation.GameFeaturesNotAdded,
                            type: 'warning'
                        });
                    }
                });
            };

            //translation
            $scope.translateGameFeatures = function (targetLanguage, gameFeatures) {
                var translateLanguageModel = {
                    CurrentLanguageCode: $scope.getTemplateLanguageShortName(),
                    TargetLanguageCode: targetLanguage.LanguageShortName,
                    ContentForTranslation: []
                };
                gameFeatures.forEach(function (x) {
                    translateLanguageModel.ContentForTranslation.push(x.GameFeatureTemplateName);
                });
                translateService.PostLanguageTranslation(translateLanguageModel).then(function (x) {
                    if (x.IsValid) {
                        for (var i = 0; i < gameFeatures.length; i++) {
                            $scope.gameFeatures[i].GameFeatureName = x.Data.Result.text[i];
                        }
                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.TranslationCompletedSuccessfully,
                            type: 'success'
                        });
                    } else {
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: $scope.translation.ErrorDuringTranslation + ' ' + x.ErrorMessage,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            $scope.translateGameFeaturesDetails = function (targetLanguage, gameFeaturesDetails) {
                var translateLanguageModel = {
                    CurrentLanguageCode: $scope.getTemplateLanguageShortName(),
                    TargetLanguageCode: targetLanguage.LanguageShortName,
                    ContentForTranslation: []
                };
                gameFeaturesDetails.forEach(function (x) {
                    translateLanguageModel.ContentForTranslation.push(x.GameFeatureDetailTemplateName);
                });
                translateService.PostLanguageTranslation(translateLanguageModel).then(function (x) {
                    if (x.IsValid) {
                        for (var i = 0; i < gameFeaturesDetails.length; i++) {
                            $scope.gameFeatureDetails[i].GameFeatureDetailName = x.Data.Result.text[i];
                        }

                        $.Notify({
                            caption: $scope.translation.Success,
                            content: $scope.translation.TranslationCompletedSuccessfully,
                            type: 'success'
                        });
                    } else {
                        $.Notify({
                            caption: $scope.translation.Failure,
                            content: $scope.translation.ErrorDuringTranslation + ' ' + x.ErrorMessage,
                            type: 'alert',
                            timeout: 10000
                        });
                    }
                });
            };
            $scope.getTemplateLanguageShortName = function () {
                var templateLanguageShortName = '';
                $scope.languages.forEach(function (x) {
                    if (x.LanguageName === "English")
                        templateLanguageShortName = x.LanguageShortName;
                });
                return templateLanguageShortName;
            };

            //base init
            function initContent(language) {
                if (language !== undefined) {
                    $scope.getGameFeatureDetails(language);
                }
            };
            initContent($scope.selectedLanguage);
            // ********************

        }]);
})();
