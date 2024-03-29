﻿(function () {
    angular
        .module('webBellwether')
        .controller('signupController', ['$scope', '$location', '$timeout', 'authService','sharedService', function ($scope, $location, $timeout, authService,sharedService) {

            $scope.savedSuccessfully = false;
            $scope.message = "";
            var messageTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $scope.message = '';
                }, 4000);
            };

            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    sharedService.prepForPublish(false);
                    console.log($scope.message.toString());
                    $scope.message = '';
                }, 2000);
            }

            $scope.registration = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            $scope.signUp = function () {

                authService.saveRegistration($scope.registration).then(function (response) {

                    $scope.savedSuccessfully = true;
                    $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                        $scope.registration = {
                            userName: "",
                            password: "",
                            confirmPassword: ""
                        };
                    startTimer();

                },
                 function (response) {
                     var errors = [];
                     for (var key in response.data.modelState) {
                         for (var i = 0; i < response.data.modelState[key].length; i++) {
                             errors.push(response.data.modelState[key][i]);
                         }
                     }
                     $scope.message = "Failed to register user due to:" + errors.join(' ');
                     messageTimer();
                 });
            };

            

        }]);
})();
