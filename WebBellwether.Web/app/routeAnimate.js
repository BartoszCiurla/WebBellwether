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
        .run(function($rootScope, $location) {
            $rootScope.$on("$routeChangeStart", function(event, next, current) {
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

                $rootScope.transitionState = 'slide-pop'; //getRandomAnimation();
                //aby działał footer musze poprawić animacje od teraz page nie bedzie posiadalo position absolute 
                //bede je dodawal tylko na potrzeby animacji i w czasie ich trwania gdy bede mail stan active ustawiam relative 
                //mimo wszystko nie dziala to zbyt dobrze bo na 1 sekunde widac footer ... juz poprawione 

            });

        });
})();


//(function () {
//    angular
//        .module('webBellwether')
//        .directive("fixPositionAbsolute", ["$document", "$window", function($document, $window) {
//            return {
//                restrict: "A",
//                link: function($scope, element) {
//                    // Fleg to determine if the directive has loaded before
//                    var hasLoaded;
//                    // DOM representation of the Angular element
//                    var domElement = element[0];
//                    $scope.$on("$routeChangeSuccess", function() {
//                        console.log(domElement);
//                        // Get the computed height of the ui-view and assign it to the directive element
//                        domElement.style.height = $window.getComputedStyle($document[0].querySelector("ng-view")).height;
//                        // After the first height change, add a class to enable animations from now on
//                        if(!hasLoaded) {
//                            domElement.classList.add("auto-height");
//                            hasLoaded = true;
//                        }
//                    });
//                }
//            };
//        }]);
//

       