(function () {
    angular
        .module('webBellwether')
        .controller('managementVersionsController', ['$scope', '$timeout', 'versionsService', function ($scope, $timeout, versionsService) {
            $scope.languageForVersion = null;
            $scope.versionDetail = {};
            $scope.getVersionDetailsForLanguage = function (language) {
                if (language !== undefined)
// ReSharper disable once QualifiedExpressionMaybeNull
                    if (language.id !== undefined)
                versionsService.GetVersionDetailsForLanguage(language.id).then(function (x) {
                    $scope.versionDetail = x.data;
                }, function() {

                });
            };
            $scope.addVersion = function(langVersion,versionTarget, numberOf) {
                var versionModel = {
                    VersionNumber: langVersion,
                    NumberOf: numberOf,
                    LanguageId: $scope.languageForVersion.id,
                    VersionTarget: versionTarget
                }
                versionsService.PostNewVersion(versionModel).then(function() {
                    selectTableVersion(versionModel,true);
                }, function() {

                });
            };
            $scope.removeVersion = function (version, target,index) {
                var versionForDelete = {
                    Id:version.id,
                    VersionTarget: target,
                    Index: index,
                    VersionNumber:version.versionNumber,
                    NumberOf: version.numberOf,
                    LanguageId: $scope.languageForVersion.id
                }
                versionsService.PostRemoveVersion(versionForDelete).then(function() {
                    selectTableVersion(versionForDelete, false);
                }, function() {

                });
            };
            function selectTableVersion(versionModel,addTrueRemoveFalse) {
                if (versionModel.VersionTarget === "language")
                    addTrueRemoveFalse ? insertVersionToLanguage(versionModel) : removeVersionFromLanguage(versionModel.Index);
                if (versionModel.VersionTarget === "integrationGame")
                    addTrueRemoveFalse ? insertVersionToIntegrationGame(versionModel) : removeVersionFromIntegrationGame(versionModel.Index);
                if (versionModel.VersionTarget === "jokeCategory")
                    addTrueRemoveFalse ? insertVersionToJokeCategory(versionModel) : removeVersionFromJokeCategory(versionModel.Index);
                if (versionModel.VersionTarget === "joke")
                    addTrueRemoveFalse ? insertVersionToJoke(versionModel) : removeVersionFromJoke(versionModel.Index);
            };
            function removeVersionFromLanguage(index) {
                $scope.versionDetail.languageVersions.splice(index,1);
            };

            function removeVersionFromIntegrationGame(index) {
                $scope.versionDetail.integrationGameVersions.splice(index,1);
            };

            function removeVersionFromJokeCategory(index) {
                $scope.versionDetail.jokeCategoryVersions.splice(index,1);
            };
            function removeVersionFromJoke(index) {
                $scope.versionDetail.jokeVersions.splice(index,1);
            };

            function insertVersionToLanguage(newVersionModel) {
                $scope.versionDetail.languageVersions.push(createVersionDetail(newVersionModel));
            };
            function insertVersionToIntegrationGame(newVersionModel) {
                $scope.versionDetail.integrationGameVersions.push(createVersionDetail(newVersionModel));
            };
            function insertVersionToJokeCategory(newVersionModel) {
                $scope.versionDetail.jokeCategoryVersions.push(createVersionDetail(newVersionModel));
            };
            function insertVersionToJoke(newVersionModel) {
                $scope.versionDetail.jokeVersions.push(createVersionDetail(newVersionModel));
            };
            function createVersionDetail(newVersionModel) {
                var versionDetail = {
                    versionNumber: newVersionModel.VersionNumber,
                    numberOf:newVersionModel.NumberOf
                }
                return versionDetail;
            }
        }]);
})();

