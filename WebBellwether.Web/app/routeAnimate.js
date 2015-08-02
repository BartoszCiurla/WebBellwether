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