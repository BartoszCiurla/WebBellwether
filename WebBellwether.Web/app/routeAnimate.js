(function () {
    angular
        .module('webBellwether')
        .config(function ($routeProvider) {
            $routeProvider.when("/home", {
                controller: "homeController",
                templateUrl: "/app/home/home.html"                    
            });
            $routeProvider.when("/game", {
                controller: "integrationGameController",
                templateUrl:"/app/integrationGames/integrationGame.html"
            });
            $routeProvider.when("/managementIntegrationGame", {
                controller: "integrationGameManagementController",
                templateUrl: "/app/integrationGames/integrationGameManagement.html"
            });
            $routeProvider.when("/gameFeatureManagement", {
                controller: "gameFeatureManagementController",
                templateUrl: "/app/integrationGames/gameFeatureManagement.html"
            });
            $routeProvider.when("/jokeManagement", {
                controller: "jokeManagementController",
                templateUrl: "/app/jokes/jokeManagement.html"
            });
            $routeProvider.when("/jokeCategoryManagement", {
                controller: "jokeCategoryManagementController",
                templateUrl: "/app/jokes/jokeCategoryManagement.html"
            });
            $routeProvider.when("/languageManagement", {
                controller: "languageManagementController",
                templateUrl: "/app/languages/languageManagement.html"
            });
            $routeProvider.when("/versionManagement", {
                controller: "versionManagementController",
                templateUrl: "/app/versions/versionManagement.html"
            });
            $routeProvider.when("/bellwether", {
                controller: "bellwetherController",
                templateUrl: "/app/bellwether/bellwether.html"
            });
            $routeProvider.when("/joke", {
                controller:"jokeController",
                templateUrl: "/app/jokes/joke.html"
            });
            $routeProvider.when("/photoGallery", {
                controller:"photoGalleryController",
                templateUrl: "/app/photoGalleries/photoGallery.html"
            });
            $routeProvider.when("/guide", {
                controller:"guideController",
                templateUrl: "/app/guide/guide.html"
            });            

            $routeProvider.when("/refresh", {
                controller: "refreshController",
                templateUrl: "/app/authorizations/refresh.html"
            });

            $routeProvider.when("/tokens", {
                controller: "tokensManagerController",
                templateUrl: "/app/authorizations/tokens.html"
            });

            $routeProvider.when("/associate", {
                controller: "associateController",
                templateUrl: "/app/authorizations/associate.html"
            });

            $routeProvider.otherwise({ redirectTo: "/home" });

        });
    
})();

(function () {
    angular
        .module('webBellwether')
        .run(function ($rootScope, $location) {            
            $rootScope.$on("$routeChangeStart", function(event, next, current) {
                $('footer').addClass("marginTop1000");
            });

            function getRandomAnimation() {
                var rnd = Math.floor((Math.random() * 5) + 1);
                switch (rnd) {
                    case 1:
                        return 'slide-pop';
                    case 2:
                        return 'page-slideInRight';
                    case 3:
                        return 'page-rON';
                    case 4:
                        return 'page-rotateFall';
                    case 5:
                        return 'fade';
                    default:
                        return 'page-rON';
                }
            };

            $rootScope.$on("$routeChangeSuccess", function (event, next, current) {             
                $('#myView').removeClass();
                $('#myView').addClass('page');
                $('#myView').addClass(getRandomAnimation());
            });
          
        });
})();


(function() {
    angular
        .module('webBellwether')
        .directive("fixPositionAbsolute", [
            "$rootScope", "$document", "$window", function ( $rootScope,$document, $window) {
                return {
                    restrict: "A",
                    link: function ($rootScope) {
                        $rootScope.$on("$routeChangeSuccess", function () {
                            setTimeout(function () {
                                $('footer').removeClass("marginTop1000");
                            },1500);
                        });
                    }
                };
            }
        ]);
})();
       