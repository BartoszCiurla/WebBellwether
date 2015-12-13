(function () {
    angular
        .module('webBellwether')
        .config(function ($routeProvider) {
            $routeProvider.when("/home", {
                controller: "homeController",
                templateUrl: "/app/home/home.html"                    
            });
            $routeProvider.when("/games", {
                controller: "integrationGamesController",
                templateUrl:"/app/integrationGames/integrationGames.html"
            });
            $routeProvider.when("/managementIntegrationGames", {
                controller: "managementIntegrationGamesController",
                templateUrl: "/app/integrationGames/managementIntegrationGames.html"
            });
            $routeProvider.when("/editGameFeatures", {
                controller: "editGameFeaturesController",
                templateUrl: "/app/integrationGames/editGameFeatures.html"
            });
            $routeProvider.when("/managementJokes", {
                controller: "managementJokesController",
                templateUrl: "/app/jokes/managementJokes.html"
            });
            $routeProvider.when("/managementJokeCategories", {
                controller: "managementJokeCategoriesController",
                templateUrl: "/app/jokes/managementJokeCategories.html"
            });
            $routeProvider.when("/managementLanguages", {
                controller: "managementLanguagesController",
                templateUrl: "/app/languages/managementLanguages.html"
            });
            $routeProvider.when("/bellwether", {
                controller: "bellwetherController",
                templateUrl: "/app/bellwether/bellwether.html"
            });
            $routeProvider.when("/jokes", {
                controller:"jokesController",
                templateUrl: "/app/jokes/jokes.html"
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
       