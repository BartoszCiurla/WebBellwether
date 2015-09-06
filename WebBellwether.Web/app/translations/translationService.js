(function () {
    angular
        .module('webBellwether')
        .service("translationService", function ($resource, $http) {
            this.getLanguages = function () {
                console.log("get languages");
            return $http.get("/api/Language");

        };
            this.getTranslation = function ($scope, language) {
                var languageFilePath ='/appData/translations/translation_' + language + '.json';
                console.log(languageFilePath);
                $resource(languageFilePath).get(function (data) {
                    $scope.translation = data;
                });
            };
        });
})();