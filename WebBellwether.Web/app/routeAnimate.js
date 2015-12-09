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

//kurde z tego poziomu to nei zadziała 12 godzin poszlo ... musze serwis napisac do tego i wstrzykiwać do kazdego kontrollera ...
//jednak sprawa lezala po stronie angulara tylko wersja 1.2.13 pozwala na taka opcję , wiec migracja , serwis usuniety
(function () {
    angular
        .module('webBellwether')
        .run(function ($rootScope, $location) {            
            $rootScope.$on("$routeChangeStart", function(event, next, current) {
                $('footer').addClass("hide"); //z racji ze nie wiem jak to ogarnac poprostu ukryje footer na czas animacji ;)             
//aby działał footer musze poprawić animacje od teraz page nie bedzie posiadalo position absolute 
                //bede je dodawal tylko na potrzeby animacji i w czasie ich trwania gdy bede mail stan active ustawiam relative 
                //mimo wszystko nie dziala to zbyt dobrze bo na 1 sekunde widac footer ... juz poprawione 
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
                        //najprostrze rozwiązania bywają najlepszymi 
                        //w przyszlości mozna by się zastanowić nad porobieniem takich opoznien czasowych w zaleznosci od kontentu 
                        $rootScope.$on("$routeChangeSuccess", function () {
                            setTimeout(function () {
                                $('footer').removeClass("hide");
                            },1500);
                        });
                    }
                };
            }
        ]);
})();
       