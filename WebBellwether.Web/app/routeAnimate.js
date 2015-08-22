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
                templateUrl: "/app/views/bellwether.html"
            });
            $routeProvider.when("/jokes", {
                templateUrl: "/app/views/jokes.html"
            });
            $routeProvider.when("/photoGallery", {
                templateUrl: "/app/views/photoGallery.html"
            });
            $routeProvider.when("/guide", {
                templateUrl: "/app/views/guide.html"
            });

            //$routeProvider.when("/login", {
            //    //controller: "loginController",
            //    templateUrl: "/app/views/login.html"
            //});
            //$routeProvider.when("/login", {
            //    controller: "loginController",
            //    templateUrl: "/app/views/login.html"
            //});

            //$routeProvider.when("/signup", {
            //    controller: "signupController",
            //    templateUrl: "/app/views/signup.html"
            //});

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
(function () {
    angular
        .module('webBellwether')
        .run(function($rootScope, $location) {
            $rootScope.$on("$routeChangeStart", function (event, next, current) {           
                function getRandomAnimation() {
                    var rnd = Math.floor((Math.random() * 10) + 1);
                    switch (rnd) {
                        case 1:
                            return 'slide-pop';
                        case 2:
                            return 'fade';
                        case 3:
                            return 'page-rON';
                        case 4:
                            return 'fade';
                        case 5:
                            return 'page-slideInRight';
                        case 6:
                            return 'page-rotateFall';
                        case 7:
                            return 'slide-pop';
                        case 8:
                            return 'fade';
                        case 9:
                            return 'page-rON';
                        case 10:
                            return 'fade';
                        default:
                            return 'page-rON';
                    }
                    //switch (rnd) {
                    //case 1:
                    //    return 'page-home';
                    //    case 2:
                    //        return 'page-contact';
                    //    case 3:
                    //        return 'page-about';
                    //default:
                    //    return 'page-contact';
                    //}
                };

                $rootScope.transitionState = getRandomAnimation();
                //$rootScope.transitionState = 'pt-page-moveToLeft ' + 'pt-page-moveFromRight';
                //zacznij od tego aby wyczaic o co w tym chodzi 
                //działające to :
                //slide-pop
                //fade bez position absolute działa 
                //page-rON
                //page-slideInRight
                //page-rotateFall
            });
        });

})();

       