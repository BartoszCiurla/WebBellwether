(function () {
    angular
        .module('webBellwether')
        .controller('versionManagementController', ['$scope', '$timeout', 'versionsService', function ($scope, $timeout, versionsService) {
            $scope.languageForVersion = null;
            $scope.versionDetail = {};
            $scope.getVersionDetailsForLanguage = function (language) {
                if (language.Id !== undefined)
                    if (language.Id !== null)
                        versionsService.GetVersionDetailsForLanguage(language.Id).then(function (x) {
                            if (x.data.IsValid) {
                                $scope.versionDetail = {};
                                $scope.versionDetail = x.data.Data;
                            }
                                
                        });
            };
            $scope.addVersion = function (langVersion, versionTarget, numberOf) {
                var versionModel = {
                    VersionNumber: langVersion,
                    NumberOf: numberOf,
                    LanguageId: $scope.languageForVersion.Id,
                    VersionTarget: versionTarget
                }
                versionsService.PostNewVersion(versionModel).then(function (x) {
                    if (x.data.IsValid)
                        selectTableVersion(versionModel, true);
                });
            };
            $scope.removeVersion = function (version, target, index) {
                var versionForDelete = {
                    Id: version.Id,
                    VersionTarget: target,
                    Index: index,
                    VersionNumber: version.VersionNumber,
                    NumberOf: version.NumberOf,
                    LanguageId: $scope.languageForVersion.Id
                }
                versionsService.PostRemoveVersion(versionForDelete).then(function (x) {
                    if (x.data.IsValid)
                        selectTableVersion(versionForDelete, false);
                });
            };
            function selectTableVersion(versionModel, addTrueRemoveFalse) {
                if (versionModel.VersionTarget === "language")
                    addTrueRemoveFalse ? insertVersionToLanguage(versionModel) : removeVersionFromLanguage(versionModel.Index);
                if (versionModel.VersionTarget === "integrationGame")
                    addTrueRemoveFalse ? insertVersionToIntegrationGame(versionModel) : removeVersionFromIntegrationGame(versionModel.Index);
                if (versionModel.VersionTarget === "jokeCategory")
                    addTrueRemoveFalse ? insertVersionToJokeCategory(versionModel) : removeVersionFromJokeCategory(versionModel.Index);
                if (versionModel.VersionTarget === "joke")
                    addTrueRemoveFalse ? insertVersionToJoke(versionModel) : removeVersionFromJoke(versionModel.Index);
                if (versionModel.VersionTarget === "gameFeature")
                    addTrueRemoveFalse ? insertVersionToGameFeature(versionModel) : removeVersionFromGameFeature(versionModel.Index);
            };

            function removeVersionFromGameFeature(index) {
                $scope.versionDetail.GameFeatureVersions.splice(index, 1);
            }

            function removeVersionFromLanguage(index) {
                $scope.versionDetail.LanguageVersions.splice(index, 1);
            };

            function removeVersionFromIntegrationGame(index) {
                $scope.versionDetail.IntegrationGameVersions.splice(index, 1);
            };

            function removeVersionFromJokeCategory(index) {
                $scope.versionDetail.JokeCategoryVersions.splice(index, 1);
            };
            function removeVersionFromJoke(index) {
                $scope.versionDetail.JokeVersions.splice(index, 1);
            };

            function insertVersionToGameFeature(newVersionModel) {
                $scope.versionDetail.GameFeatureVersions.push(createVersionDetail(newVersionModel));
            }

            function insertVersionToLanguage(newVersionModel) {
                $scope.versionDetail.LanguageVersions.push(createVersionDetail(newVersionModel));
            };
            function insertVersionToIntegrationGame(newVersionModel) {
                $scope.versionDetail.IntegrationGameVersions.push(createVersionDetail(newVersionModel));
            };
            function insertVersionToJokeCategory(newVersionModel) {
                $scope.versionDetail.JokeCategoryVersions.push(createVersionDetail(newVersionModel));
            };
            function insertVersionToJoke(newVersionModel) {
                $scope.versionDetail.JokeVersions.push(createVersionDetail(newVersionModel));
            };
            function createVersionDetail(newVersionModel) {
                var versionDetail = {
                    VersionNumber: newVersionModel.VersionNumber,
                    NumberOf: newVersionModel.NumberOf
                }
                return versionDetail;
            }
        }]);
})();

