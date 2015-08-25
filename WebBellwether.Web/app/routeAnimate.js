(function () {
    angular
        .module('webBellwether')
        .config(function ($routeProvider) {
            $routeProvider.when("/home", {
                controller: "homeController",
                templateUrl: "/app/views/home.html"                    
            });
            $routeProvider.when("/games", {
                controller: "integrationGamesController",
                templateUrl:"/app/views/integrationGames.html"
            });
            $routeProvider.when("/managementIntegrationGames", {
                controller: "integrationGamesController",
                templateUrl: "/app/views/managementIntegrationGames.html"
            });
            $routeProvider.when("/managementJokes", {
                controller: "integrationGamesController",
                templateUrl: "/app/views/managementJokes.html"
            });
            $routeProvider.when("/bellwether", {
                controller: "bellwetherController",
                templateUrl: "/app/views/bellwether.html"
            });
            $routeProvider.when("/jokes", {
                controller:"jokesController",
                templateUrl: "/app/views/jokes.html"
            });
            $routeProvider.when("/photoGallery", {
                controller:"photoGalleryController",
                templateUrl: "/app/views/photoGallery.html"
            });
            $routeProvider.when("/guide", {
                controller:"guideController",
                templateUrl: "/app/views/guide.html"
            });
            $routeProvider.when("/orders", {
                controller: "ordersController",
                templateUrl: "/app/views/orders.html"
            });

            $routeProvider.when("/refresh", {
                controller: "refreshController",
                templateUrl: "/app/views/refresh.html"
            });

            $routeProvider.when("/tokens", {
                controller: "tokensManagerController",
                templateUrl: "/app/views/tokens.html"
            });

            $routeProvider.when("/associate", {
                controller: "associateController",
                templateUrl: "/app/views/associate.html"
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
            $rootScope.$on("$routeChangeSuccess", function (event, next, current) {
                // $rootScope.transitionState = getRandomAnimation();
                function getRandomAnimation() {
                    var rnd = Math.floor((Math.random() * 6) + 1);
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
       